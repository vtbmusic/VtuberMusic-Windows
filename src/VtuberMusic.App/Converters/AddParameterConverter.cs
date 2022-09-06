using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class AddParameterConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is string text && parameter is string par) {
            return text += par;
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
