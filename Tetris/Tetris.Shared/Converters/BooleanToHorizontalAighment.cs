using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Tetris.Converters
{
    public class BooleanToHorizontalAighment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolean = System.Convert.ToBoolean(value);
            return boolean ? HorizontalAlignment.Left : HorizontalAlignment.Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
