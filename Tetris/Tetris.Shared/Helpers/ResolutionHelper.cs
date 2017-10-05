#region Using statements

using Windows.UI.Xaml;
using Tetris.Models.Core;
using Tetris.Models.Scene;

#endregion

namespace Tetris.Helpers
{
    static class ResolutionHelper
    {
        public static ViewConfig GetViewConfig(int colorIndex = 0)
        {
            var width = Window.Current.Bounds.Width;
#if WINDOWS_PHONE_APP
            var blocksize = (width / 11) * 7 / 10;
            var previewBlockSize = ((width - 6) / 56) * 15 / 10;
#endif
#if WINDOWS_APP
            var height = Window.Current.Bounds.Height;
            var blocksize = (height - 40) / 20;
            var previewBlockSize = ((width - 6) / 56) * 15 / 10; 
#endif
            return new ViewConfig(blocksize, previewBlockSize, colorIndex);
        }
    }
}