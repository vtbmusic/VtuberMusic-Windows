using System;
using System.Collections.Generic;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Models;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class ArtistStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return MusicHelepr.GetArtistString(value as IEnumerable<Artist>);

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
