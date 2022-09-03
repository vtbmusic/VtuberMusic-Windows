using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using Windows.Media.Playback;

namespace VtuberMusic.App.Converters;
public class PlayerStateConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, string language) => (MediaPlaybackState)value switch {
        MediaPlaybackState.Playing => new SymbolIcon(Symbol.Pause),
        _ => new SymbolIcon(Symbol.Play),
    };

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
