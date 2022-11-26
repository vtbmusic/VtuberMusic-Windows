using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;

public class ZeroVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is not int intValue) {
            return Visibility.Collapsed;
        }

        if (intValue == 0)
            return Visibility.Collapsed;
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        return DependencyProperty.UnsetValue;
    }
}