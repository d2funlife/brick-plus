using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data; 

namespace Tetris.Converters
{
    public class BooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = System.Convert.ToBoolean(value);
            var isInvert = parameter != null;
            if (isInvert)
                boolValue = !boolValue;
            
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}