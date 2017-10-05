using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace Tetris.Models.Core
{
    public class Audio
    {
        private readonly AudioBuffer buffer;
        private readonly SoundStream soundstream;
        private readonly SourceVoice sourceVoice;

        public Audio(string soundPath)
        {
            var xaudio = new XAudio2();
            var masteringsound = new MasteringVoice(xaudio);
            var nativefilestream = new NativeFileStream(soundPath, NativeFileMode.Open, NativeFileAccess.Read, NativeFileShare.Read);
            soundstream = new SoundStream(nativefilestream);
            var waveFormat = soundstream.Format;
            buffer = new AudioBuffer
            {
                Stream = soundstream.ToDataStream(),
                AudioBytes = (int)soundstream.Length,
                Flags = BufferFlags.EndOfStream
            };
            sourceVoice = new SourceVoice(xaudio, waveFormat, true);
            sourceVoice.ExitLoop();
        }
        public void Play()
        {
            sourceVoice.Stop();
            sourceVoice.SubmitSourceBuffer(buffer, soundstream.DecodedPacketsInfo);
            sourceVoice.Start();
        }

        public void Destroy()
        {
            sourceVoice.DestroyVoice();
        }
    }
}