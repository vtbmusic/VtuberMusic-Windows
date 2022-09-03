using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class NullVisibilityConvter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => value != null ? Visibility.Visible : (object)Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, string language) => DependencyProperty.UnsetValue;
}
