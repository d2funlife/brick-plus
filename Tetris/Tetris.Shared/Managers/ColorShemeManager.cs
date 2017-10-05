#region Usign statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models.Core;

#endregion

namespace Tetris.Managers
{
    public static class ColorShemeManager
    {
        public static async Task<List<ColorSheme>> GetColorShemesAsync()
        {
            var colorsShemes = new List<ColorSheme>();
            foreach (var colorSheme in GameConfig.ColorShemes)
                colorsShemes.Add(new ColorSheme(colorSheme));
            return colorsShemes;
        }
    }
}