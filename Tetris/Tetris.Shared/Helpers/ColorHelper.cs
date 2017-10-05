using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace Tetris.Helpers
{
    public class ColorHelper
    {
        public static Color? GetColor(string color)
        {
            if (String.IsNullOrEmpty(color) || color.Length != 9) return null;
            var r = (byte)(Convert.ToUInt32(color.Substring(3, 2), 16));
            var g = (byte)(Convert.ToUInt32(color.Substring(5, 2), 16));
            var b = (byte)(Convert.ToUInt32(color.Substring(7, 2), 16));
            return Color.FromArgb(255, r, g, b);
        }
    }
}