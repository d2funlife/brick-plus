using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tetris.Converters
{
    public class BooleanToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null || value == null) return null;
            var boolValue = System.Convert.ToBoolean(value);
            var resourceName = boolValue ? parameter : "No" + parameter;
            return Application.Current.Resources[resourceName];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}