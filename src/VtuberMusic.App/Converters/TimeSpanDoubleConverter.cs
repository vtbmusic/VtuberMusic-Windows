﻿using System;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class TimeSpanDoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return ((TimeSpan)value).TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return TimeSpan.FromSeconds((double)value);
        }
    }
}
