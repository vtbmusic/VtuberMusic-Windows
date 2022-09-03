using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class FormatConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => value == null ? null : parameter == null ? value : string.Format((string)parameter, value);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
