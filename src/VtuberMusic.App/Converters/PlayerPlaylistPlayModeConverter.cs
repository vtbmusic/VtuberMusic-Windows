using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using VtuberMusic.AppCore.Enums;

namespace VtuberMusic.App.Converters;
public class PlayerPlaylistPlayModeConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => (PlaylistPlayMode)value switch {
        PlaylistPlayMode.PlaylistRepeat => new SymbolIcon(Symbol.RepeatAll),
        PlaylistPlayMode.SingleRepeat => new SymbolIcon(Symbol.RepeatOne),
        PlaylistPlayMode.Shuffle => new SymbolIcon(Symbol.Shuffle),
        _ => null,
    };

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
