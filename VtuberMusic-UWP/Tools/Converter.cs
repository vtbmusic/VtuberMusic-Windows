using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VtuberMusic_UWP.Tools
{
    public class TimeConver : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null && value.GetType() != typeof(double))
                return DependencyProperty.UnsetValue;

            return TimeSpan.FromSeconds((float)value).ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class LikeMusicIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null && value.GetType() == typeof(bool))
            {
                if ((bool)value) return "\uE00B";
            }

            return "\uE006";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class LyricHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null && value.GetType() == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(((string)value))) return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class PlaylistItemVisibilityConver : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value == null && value.GetType() != typeof(bool))
                return DependencyProperty.UnsetValue;

            if ((bool)value)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
