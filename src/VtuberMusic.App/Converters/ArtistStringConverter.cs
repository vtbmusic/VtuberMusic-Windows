using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.Converters;
public class ArtistStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => MusicHelepr.GetArtistString(value as IEnumerable<Artist>);

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
