#region Using statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tetris.Models.Core;

#endregion

namespace Tetris.Managers
{
    public static class ControlPositionManager
    {
        public static ObservableCollection<ControlPosition> GetPositions()
        {
            var positions = new ObservableCollection<ControlPosition>();
            foreach (var item in GameConfig.Positions)
                positions.Add(new ControlPosition(item));
            return positions;
        }

        public static ObservableCollection<ControlMargin> GetMargins()
        {
            var margins = new ObservableCollection<ControlMargin>();
            foreach(var item in GameConfig.Margins)
                margins.Add(new ControlMargin(item));
            return margins;
        }
    }
}