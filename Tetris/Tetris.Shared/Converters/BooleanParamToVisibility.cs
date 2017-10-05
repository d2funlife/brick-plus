using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tetris.Converters
{
    public class BooleanParamToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = System.Convert.ToBoolean(value);

            if (parameter != null)
            {
                var boolParameter = System.Convert.ToBoolean(parameter);
                return (boolValue == boolParameter) ? Visibility.Visible : Visibility.Collapsed;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}