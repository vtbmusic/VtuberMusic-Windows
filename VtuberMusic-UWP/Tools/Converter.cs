using System;
using System.Collections;
using System.Collections.Generic;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace VtuberMusic_UWP.Tools {
    public class ListViewItemIndexConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var presenter = value as ListViewItemPresenter;
            var item = VisualTreeHelper.GetParent(presenter) as ListViewItem;

            var listView = ItemsControl.ItemsControlFromItemContainer(item);
            int index = listView.IndexFromContainer(item) + 1;
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class NullVisabilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value == null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class ProfileExpStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value != null && value.GetType() == typeof(Profile)) {
                var profile = (Profile)value;
                return profile.nextexperience.GetValueOrDefault() == 0
                ? $"{ profile.experience.GetValueOrDefault().ToString() } / -"
                : $"{ profile.experience.GetValueOrDefault().ToString() } / { profile.nextexperience.GetValueOrDefault().ToString() }";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class ArtistStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value != null && value.GetType() == typeof(Artist[])
                ? UsefullTools.GetArtistsString((Artist[])value)
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class PlayStateSymbolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value != null && value.GetType() == typeof(MediaPlaybackState)
                ? (MediaPlaybackState)value == MediaPlaybackState.Playing
                    ? Symbol.Pause
                    : Symbol.Play
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class TimeSpanStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value == null && value.GetType() != typeof(TimeSpan)
                ? DependencyProperty.UnsetValue
                : ( (TimeSpan)value ).ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class DoubleTimeStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value == null && value.GetType() != typeof(float)
                ? DependencyProperty.UnsetValue
                : TimeSpan.FromSeconds((int)(float)value).ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class GetTimeSpanMsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value != null && value.GetType() == typeof(TimeSpan)
                ? ( (TimeSpan)value ).TotalMilliseconds
                : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class LikeMusicIconConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            if (value != null && value.GetType() == typeof(bool)) {
                if ((bool)value) return "\uE00B";
            }

            return "\uE006";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class LyricHideConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            if (value != null && value.GetType() == typeof(string)) {
                if (string.IsNullOrWhiteSpace(( (string)value ))) return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class PlaylistItemVisibilityConver : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            return value == null && value.GetType() != typeof(bool)
                ? DependencyProperty.UnsetValue
                : (bool)value ? Visibility.Visible : (object)Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
