using System;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class DoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value == null ? 0.0 : (object)System.Convert.ToDouble((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
