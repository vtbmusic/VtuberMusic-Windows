using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters {
    public class DoubleTimeSpanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return parameter != null
                ? string.Format(parameter as string, TimeSpan.FromSeconds((value as double?).GetValueOrDefault()))
                : (object)TimeSpan.FromSeconds((value as double?).GetValueOrDefault());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return (value as TimeSpan?).GetValueOrDefault().TotalSeconds;
        }
    }
}
