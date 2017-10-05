using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.ApplicationInsights;
using Tetris.Enums;
using Tetris.Models.Blocks;
using Tetris.Models.Scene;
using Tetris.Models.Suspend;

namespace Tetris.Common
{
    public sealed class SuspensionManager
    {
        private static Dictionary<string, object> sessionState = new Dictionary<string, object>();
        private static List<Type> _knownTypes = new List<Type> { typeof(SuspendingGameEngine), typeof(SuspendingSession), typeof(SuspendingBoardBlock), typeof(SuspendingGame), typeof(SuspendingBlock), typeof(GridIndex), typeof(GameStatus), typeof(Mode), typeof(BaseBlock), typeof(I), typeof(J), typeof(L), typeof(O), typeof(S), typeof(T), typeof(Z) };

        private const string SessionStateFilename = "SuspensionData.xml";

        private static DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof(string), typeof(SuspensionManager), null);
        private static DependencyProperty FrameSessionBaseKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionBaseKeyParams", typeof(string), typeof(SuspensionManager), null);
        private static DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_FrameSessionState", typeof(Dictionary<string, object>), typeof(SuspensionManager), null);
        private static List<WeakReference<Frame>> registeredFrames = new List<WeakReference<Frame>>();

        public static Dictionary<string, object> SessionState
        {
            get { return sessionState; }
        }

        public static List<Type> KnownTypes
        {
            get { return _knownTypes; }
        }

        public static async Task SaveAsync()
        {
            try
            {
                // Save the navigation state for all registered frames
                foreach (var weakFrameReference in registeredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        SaveFrameNavigationState(frame);
                    }
                }

                // Serialize the session state synchronously to avoid asynchronous access to shared
                // state
                var sessionData = new MemoryStream();
                var serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                serializer.WriteObject(sessionData, sessionState);

                // Get an output stream for the SessionState file and write the state asynchronously
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(SessionStateFilename, CreationCollisionOption.ReplaceExisting);
                using (var fileStream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                var tc = new TelemetryClient();
                tc.TrackException(e);
            }
        }

        public static async Task RestoreAsync(String sessionBaseKey = null)
        {
            sessionState = new Dictionary<String, Object>();

            try
            {
                // Get the input stream for the SessionState file
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionStateFilename);
                if (file == null) return;
                BasicProperties properties = await file.GetBasicPropertiesAsync();
                if (properties.Size == 0) return;
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>),
                        _knownTypes);
                    sessionState = (Dictionary<string, object>)serializer.ReadObject(inStream.AsStreamForRead());
                }

                // Restore any registered frames to their saved state
                foreach (var weakFrameReference in registeredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame) &&
                        (string)frame.GetValue(FrameSessionBaseKeyProperty) == sessionBaseKey)
                    {
                        frame.ClearValue(FrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception e)
            {
                var tc = new TelemetryClient();
                tc.TrackException(e);
            }
        }

        public static void RegisterFrame(Frame frame, String sessionStateKey, String sessionBaseKey = null)
        {
            if (frame.GetValue(FrameSessionStateKeyProperty) != null)
            {
                throw new InvalidOperationException("Frames can only be registered to one session state key");
            }

            if (frame.GetValue(FrameSessionStateProperty) != null)
            {
                throw new InvalidOperationException("Frames must be either be registered before accessing frame session state, or not registered at all");
            }

            if (!string.IsNullOrEmpty(sessionBaseKey))
            {
                frame.SetValue(FrameSessionBaseKeyProperty, sessionBaseKey);
                sessionStateKey = sessionBaseKey + "_" + sessionStateKey;
            }

            // Use a dependency property to associate the session key with a frame, and keep a list of frames whose
            // navigation state should be managed
            frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
            registeredFrames.Add(new WeakReference<Frame>(frame));

            // Check to see if navigation state can be restored
            RestoreFrameNavigationState(frame);
        }

        public static void UnregisterFrame(Frame frame)
        {
            // Remove session state and remove the frame from the list of frames whose navigation
            // state will be saved (along with any weak references that are no longer reachable)
            SessionState.Remove((String)frame.GetValue(FrameSessionStateKeyProperty));
            registeredFrames.RemoveAll((weakFrameReference) =>
            {
                Frame testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        public static Dictionary<String, Object> SessionStateForFrame(Frame frame)
        {
            var frameState = (Dictionary<String, Object>)frame.GetValue(FrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (String)frame.GetValue(FrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    // Registered frames reflect the corresponding session state
                    if (!sessionState.ContainsKey(frameSessionKey))
                    {
                        sessionState[frameSessionKey] = new Dictionary<String, Object>();
                    }
                    frameState = (Dictionary<String, Object>)sessionState[frameSessionKey];
                }
                else
                {
                    // Frames that aren't registered have transient state
                    frameState = new Dictionary<String, Object>();
                }
                frame.SetValue(FrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        private static void RestoreFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((String)frameState["Navigation"]);
            }
        }

        private static void SaveFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
        
        public static SuspendingSession GetSuspendingSession()
        {
            if (SessionState.Count == 0 || !SessionState.ContainsKey(SuspendingKeys.Session.ToString())) 
                return null;

            return (SuspendingSession)SessionState[SuspendingKeys.Session.ToString()];
        }
        
        public static void SaveSession(SuspendingSession session)
        {
            SessionState[SuspendingKeys.Session.ToString()] = session;            
        }

        public static SuspendingGame GetGameData()
        {
            if (SessionState.Count == 0 || !SessionState.ContainsKey(SuspendingKeys.Session.ToString()))
                return null;
            return (SuspendingGame)SessionState[SuspendingKeys.Session.ToString()];
        }

        public static void SaveGameData(SuspendingGame suspendingGame)
        {
            SessionState[SuspendingKeys.Session.ToString()] = suspendingGame;
        }
    }
}
