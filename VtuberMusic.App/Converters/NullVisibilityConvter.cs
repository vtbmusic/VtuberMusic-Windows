using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters {
    public class NullVisibilityConvter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value != null ? Visibility.Visible : (object)Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return DependencyProperty.UnsetValue;
        }
    }
}
