using System;
using Windows.UI.Xaml.Data;
using Tetris.Enums;

namespace Tetris.Converters
{
    public class RecordsTypeToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = (RecordsType)value;
            if (parameter == null) return null;
            var parameterType = (RecordsType)Enum.Parse(typeof(RecordsType), parameter.ToString(), true);
            return (type == parameterType) ? "1" : ".5";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}