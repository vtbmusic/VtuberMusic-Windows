using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters;
public class DoubleTimeSpanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => parameter != null
            ? string.Format(parameter as string, TimeSpan.FromSeconds((value as double?).GetValueOrDefault()))
            : TimeSpan.FromSeconds((value as double?).GetValueOrDefault());

    public object ConvertBack(object value, Type targetType, object parameter, string language) => (value as TimeSpan?).GetValueOrDefault().TotalSeconds;
}
