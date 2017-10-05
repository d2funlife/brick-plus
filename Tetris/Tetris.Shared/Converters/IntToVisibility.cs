using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tetris.Converters
{
    public class IntToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var mode = (int)value;
            if (parameter == null) return null;
            var parameterMode = int.Parse(parameter.ToString());
            return (mode == parameterMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}