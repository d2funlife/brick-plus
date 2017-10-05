using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Tetris.Enums;

namespace Tetris.Converters
{
    public class RecordsModeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var mode = (RecordsMode)value;
            if (parameter == null) return null;
            var parameterMode = (RecordsMode)Enum.Parse(typeof(RecordsMode), parameter.ToString(), true);
            return (mode == parameterMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}