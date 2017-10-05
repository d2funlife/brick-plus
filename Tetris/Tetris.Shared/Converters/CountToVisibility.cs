using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tetris.Converters
{
    public class CountToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null) return Visibility.Collapsed;
            var count = (int)value;
            return (count > 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return null;
        }
    }
}