#region Usign statements
using System;
using System.Collections.Generic;
using System.Text;
using Tetris.Enums;

#endregion

namespace Tetris.Models.Core
{
    public class GameSettings
    {
        public ControlType ControlType { get; set; }
        public Position ControlPosition { get; set; }
        public Margin ControlMargin { get; set; }
        public bool IsAudioAvailable { get; set; }
        public bool IsVibrateAvailable { get; set; }
        public int ColorIndex { get; set; }
        public Theme Theme{ get; set; }
        public int StartUpCount { get; set; }
        public bool IsShadowAvailable { get; set; }
        public bool IsHelpViewed { get; set; }
    }
}