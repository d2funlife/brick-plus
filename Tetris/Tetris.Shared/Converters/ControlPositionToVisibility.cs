using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Tetris.Enums;

namespace Tetris.Converters
{
    public class ControlPositionToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enumValue = (Position)Enum.Parse(typeof(Position), value.ToString());

            var parameters = parameter?.ToString().Split(',').ToArray();
            if (parameters?.Length != 2) return Visibility.Collapsed;

            var parameterValue = (Position)Enum.Parse(typeof(Position), parameters[0]);
            var parameterBool = bool.Parse(parameters[1]);

            if (enumValue == parameterValue && parameterBool)
                return Visibility.Visible;
            if (enumValue != parameterValue && !parameterBool)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}