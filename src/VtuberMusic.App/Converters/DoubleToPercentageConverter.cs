using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class DoubleToPercentageConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is double doubleValue)
            return doubleValue.ToString("P0");

        return DependencyProperty.UnsetValue;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) {
        return DependencyProperty.UnsetValue;
    }
}
