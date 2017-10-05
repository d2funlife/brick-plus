using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Tetris.Enums;

namespace Tetris.Converters
{
    public class ControlTypeToVisibility:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var valueEnum = (ControlType)value;
            if (parameter == null) return null;
            var parameterEnum = (ControlType)Enum.Parse(typeof(ControlType), parameter.ToString(), true);
            return (valueEnum == parameterEnum) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}