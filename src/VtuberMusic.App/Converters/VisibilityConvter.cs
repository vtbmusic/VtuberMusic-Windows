using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class VisibilityConvter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var boolValue = value as bool?;
            return parameter != null
                ? boolValue.GetValueOrDefault() ? Visibility.Collapsed : Visibility.Visible
                : boolValue.GetValueOrDefault() ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            var visibilityValue = value as Visibility?;
            return visibilityValue != null
                ? visibilityValue.GetValueOrDefault() == Visibility.Visible ? false : true
                : visibilityValue.GetValueOrDefault() == Visibility.Visible ? true : false;
        }
    }
}
