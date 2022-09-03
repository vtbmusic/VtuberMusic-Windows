using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class TimeSpanDoubleConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => ((TimeSpan)value).TotalSeconds;

    public object ConvertBack(object value, Type targetType, object parameter, string language) => TimeSpan.FromSeconds((double)value);
}
