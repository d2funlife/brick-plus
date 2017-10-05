#region Using statements
using System.Collections.Generic;
using Windows.UI;
using Tetris.Enums;
using Tetris.Models.Blocks;
#endregion

namespace Tetris
{
    public static class GameConfig
    {
        public static string WindowsPhoneUrl = "ms-windows-store:review?PFN:" + Windows.ApplicationModel.Package.Current.Id;

        public static string WindowsStoreUrl = "ms-windows-store:reviewapp?appid=" + Windows.ApplicationModel.Store.CurrentApp.AppId;

        public static int StartupCountUntilRate = 5;

        public const int RowsCount = 23;

        public const int ColumnsCount = 10;

        public static int ActiveRowsCount = 20;

        public const int InitializationSpeed = 500;

        public const int VibrateDuration = 50;

        public const int DefaultMoveSpeed = 120;

        public const int DefaultMoveSpeedConst = 120;

        public const int VerticalMoveSpeed = 100;

        public static readonly List<List<Color>> ColorShemes = new List<List<Color>>(5)
        {
            new List<Color>(7)
            {
                Color.FromArgb(255, 164, 146, 198),
                Color.FromArgb(255, 254, 208, 88),
                Color.FromArgb(255, 159, 207, 107),
                Color.FromArgb(255, 71, 197, 238),
                Color.FromArgb(255, 91, 194, 167),
                Color.FromArgb(255, 120, 141, 198),
                Color.FromArgb(255, 243, 109, 82)
            },
            new List<Color>(7)
            {
                Color.FromArgb(255, 246, 186, 168),
                Color.FromArgb(255, 208, 175, 206),
                Color.FromArgb(255, 196, 100, 103),
                Color.FromArgb(255, 228, 209, 62),
                Color.FromArgb(255, 156, 187, 107),
                Color.FromArgb(255, 88, 97, 119),
                Color.FromArgb(255, 73, 119, 92)
            },
            new List<Color>(7)
            {
                Color.FromArgb(255, 33, 140, 141),
                Color.FromArgb(255, 108, 206, 203),
                Color.FromArgb(255, 249, 229, 89),
                Color.FromArgb(255, 239, 113, 38),
                Color.FromArgb(255, 142, 220, 157),
                Color.FromArgb(255, 51, 102, 153),
                Color.FromArgb(255, 255, 204, 102)
            },
            new List<Color>(7)
            {
                Color.FromArgb(255, 107, 180, 185),
                Color.FromArgb(255, 216, 86, 154),
                Color.FromArgb(255, 42, 134, 109),
                Color.FromArgb(255, 234, 121, 14),
                Color.FromArgb(255, 119, 86, 183),
                Color.FromArgb(255, 233, 208, 29),
                Color.FromArgb(255, 46, 189, 238)
            },
            new List<Color>(7)
            {
                Color.FromArgb(255, 219, 201, 70),
                Color.FromArgb(255, 119, 86, 183),
                Color.FromArgb(255, 31, 131, 167),
                Color.FromArgb(255, 216, 86, 154),
                Color.FromArgb(255, 196, 73, 108),
                Color.FromArgb(255, 156, 155, 81),
                Color.FromArgb(255, 229, 226, 115)
            }
        };

        public static readonly List<Theme> Themes = new List<Theme>(3)
        {
            Theme.Auto,
            Theme.Dark,
            Theme.Light
        };

        public static readonly List<Position> Positions = new List<Position>(3)
        {
            Position.High,
            Position.Middle,
            Position.Bottom
        };

        public static readonly List<Margin> Margins = new List<Margin>(3)
        {
            Margin.Big,
            Margin.Medium,
            Margin.Small
        };

        public static readonly List<double> PositionsValue = new List<double>(3) { 20, 8, 1 };

        public static readonly List<double> MarginsValue = new List<double>(3) { 60, 30, 10 };

        public static int DisplayCountBeforeShutDown = 7;
    }
}