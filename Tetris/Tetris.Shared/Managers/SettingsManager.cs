#region Usign statements

using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Tetris.Enums;
using Tetris.Models.Core;
#endregion

namespace Tetris.Managers
{
    public static class SettingsManager
    {
        public static async Task<GameSettings> LoadSettignsAsync()
        {
            var settings = ApplicationData.Current.LocalSettings;
            return new GameSettings
            {
                ControlType = (settings.Values.ContainsKey(GameSettingsKeys.ControlType.ToString()))
                ? (ControlType)(int)settings.Values[GameSettingsKeys.ControlType.ToString()] : (ControlType)0,
                ControlPosition = (settings.Values.ContainsKey(GameSettingsKeys.ControlPosition.ToString()))
                ? (Position)(int)settings.Values[GameSettingsKeys.ControlPosition.ToString()] : (Position)1,
                ControlMargin = (settings.Values.ContainsKey(GameSettingsKeys.ControlMargin.ToString()))
                ? (Margin)(int)settings.Values[GameSettingsKeys.ControlMargin.ToString()] : (Margin)2,
                IsAudioAvailable = (settings.Values.ContainsKey(GameSettingsKeys.Audio.ToString()))
                ? (bool)settings.Values[GameSettingsKeys.Audio.ToString()] : true,
                IsVibrateAvailable = (settings.Values.ContainsKey(GameSettingsKeys.Vibrate.ToString()))
                ? (bool)settings.Values[GameSettingsKeys.Vibrate.ToString()] : true,
                ColorIndex = (settings.Values.ContainsKey(GameSettingsKeys.ColorShemeIndex.ToString()))
                ? (int)settings.Values[GameSettingsKeys.ColorShemeIndex.ToString()] : 0,
                Theme = (settings.Values.ContainsKey(GameSettingsKeys.Theme.ToString()))
                ? (Theme)(int)settings.Values[GameSettingsKeys.Theme.ToString()] : (Theme)0,
                StartUpCount = (settings.Values.ContainsKey(GameSettingsKeys.StartUpCounts.ToString()))
                ? (int)settings.Values[GameSettingsKeys.StartUpCounts.ToString()] : 0,
                IsShadowAvailable = (settings.Values.ContainsKey(GameSettingsKeys.Shadow.ToString()))
                ? (bool)settings.Values[GameSettingsKeys.Shadow.ToString()] : true,
                IsHelpViewed = (settings.Values.ContainsKey(GameSettingsKeys.IsHelpViewed.ToString()))
                ? (bool)settings.Values[GameSettingsKeys.IsHelpViewed.ToString()] : false
            };
        }
        public static async Task<string> GetPlayerNameAsync()
        {
            return (ApplicationData.Current.LocalSettings.Values.ContainsKey(GameSettingsKeys.PlayerName.ToString())) 
                ? (string)ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.PlayerName.ToString()] : "AAA";
        }

        public static double GetPositionValue(Position position)
        {
            return GameConfig.PositionsValue[(int)position];
        }

        public static double GetMarginValue(Margin margin)
        {
            return GameConfig.MarginsValue[(int)margin];
        }

        public static void SaveControlType(int index)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.ControlType.ToString()] = index;
        }

        public static void IterateStartUpCount()
        {
            var startUpCount = (ApplicationData.Current.LocalSettings.Values.ContainsKey(GameSettingsKeys.StartUpCounts.ToString()))
                ? (int)ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.StartUpCounts.ToString()] : 0;
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.StartUpCounts.ToString()] = ++startUpCount;
        }

        public static void IterateRateAppDisplayCount()
        {
            var displayCount = (ApplicationData.Current.LocalSettings.Values.ContainsKey(GameSettingsKeys.RateAppDisplayCount.ToString()))
                ? (int)ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.RateAppDisplayCount.ToString()] : 0;
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.RateAppDisplayCount.ToString()] = ++displayCount;
        }

        public static int GetRateAppDisplaysCount()
        {
            return (ApplicationData.Current.LocalSettings.Values.ContainsKey(GameSettingsKeys.RateAppDisplayCount.ToString()))
                ? (int)ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.RateAppDisplayCount.ToString()] : 0;
        }

        public static void SaveThemeIndex(int index)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.Theme.ToString()] = index;
        }

        public static void SaveColorShemeIndex(int index)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.ColorShemeIndex.ToString()] = index;
        }

        public static void SaveControlPositionIndex(int index)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.ControlPosition.ToString()] = index;
        }

        public static void SaveControlMarginIndex(int index)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.ControlMargin.ToString()] = index;
        }

        public static void SaveAudioSetting(bool isAvailable)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.Audio.ToString()] = isAvailable;
        }

        public static void SaveVibrationSettings(bool isAvailable)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.Vibrate.ToString()] = isAvailable;
        }

        public static void SetIsHelpViewedSetting()
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.IsHelpViewed.ToString()] = true;
        }

        public static void SavePlayerName(string name)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.PlayerName.ToString()] = name;
        }

        public static void SaveShadowManager(bool isShadowAvailable)
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.Shadow.ToString()] = isShadowAvailable;
        }

        public static void SaveShutDownRateNotifications()
        {
            ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.IsRateAppShoutDowned.ToString()] = true;
        }

        public static bool IsRateAppShutDowned()
        {
            return (ApplicationData.Current.LocalSettings.Values.ContainsKey(GameSettingsKeys.IsRateAppShoutDowned.ToString()))
                ? (bool)ApplicationData.Current.LocalSettings.Values[GameSettingsKeys.IsRateAppShoutDowned.ToString()] : false;
        }

        public static Dictionary<Color, Color> GetNewColorsSheme(int newIndex, int oldIndex)
        {
            var result = new Dictionary<Color, Color>();
            var newColors = GameConfig.ColorShemes[newIndex];
            var oldColors = GameConfig.ColorShemes[oldIndex];
            for (var i = 0; i < 7; i++)
            {
                result.Add(oldColors[i], newColors[i]);
            }

            return result;
        }
    }
}