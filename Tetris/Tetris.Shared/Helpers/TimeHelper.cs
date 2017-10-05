#region Using statements
using System;
using System.Collections.Generic;
using System.Text; 
#endregion

namespace Tetris.Helpers
{
    public static class TimeHelper
    {
        public static string GetTimeString(this int secondsCount)
        {
            var hours = secondsCount / 3600;
            var minutes = (secondsCount - hours * 3600) / 60;
            var seconds = secondsCount - hours * 3600 - minutes * 60;
            var hoursDozen = hours / 10;
            var hoursOne = hours % 10;
            var minutesDozen = minutes / 10;
            var minutesOne = minutes % 10;
            var secondsDozen = seconds / 10;
            var secondsOne = seconds % 10;
            return string.Format("{0}{1}:{2}{3}", minutesDozen, minutesOne, secondsDozen, secondsOne);
        }
    }
}