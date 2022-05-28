using System;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class FormatConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value == null ? null : parameter == null ? value : string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
