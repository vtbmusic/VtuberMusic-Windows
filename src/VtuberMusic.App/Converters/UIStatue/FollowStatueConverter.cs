using Microsoft.UI.Xaml.Data;
using System;

namespace VtuberMusic.App.Converters.UIStatue;
public class FollowStatueConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) {
        if (value is bool statue && statue) {
            return "已关注";
        }

        return "关注";
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
