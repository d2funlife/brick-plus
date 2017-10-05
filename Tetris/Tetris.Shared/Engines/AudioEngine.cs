using Tetris.Models.Core;

namespace Tetris.Engines
{
    public class AudioEngine
    {
        public bool IsAudioAvailable { get; set; }

        private bool isAudioAvailable;
        private bool isInitialized;
        private Audio dropDownEffect;
        private Audio removeRowEffect;
        private bool isDropDownRealize;

        private void Initialize()
        {
            dropDownEffect = new Audio(@"Assets\audio\dropdown.wav");
            removeRowEffect = new Audio(@"Assets\audio\removerow.wav");
        }

        public void SwitchAudioAvailable(bool isAvaialable)
        {
            isAudioAvailable = isAvaialable;
            if (isAvaialable)
                Initialize();
            else
            {
                dropDownEffect?.Destroy();
                removeRowEffect?.Destroy();
            }
        }

        public void PlayDropDownEffect()
        {
            if (isDropDownRealize || !isAudioAvailable) return;
            dropDownEffect.Play();
            isDropDownRealize = true;
        }

        public void UnlockDropDownEffect()
        {
            isDropDownRealize = false;
        }

        public void PlayRemoveRowEffect()
        {
            if (!isAudioAvailable) return;
            removeRowEffect.Play();
        }
    }
}