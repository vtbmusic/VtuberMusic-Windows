using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using Windows.Media.Playback;

namespace VtuberMusic.App.Converters {
    public class PlayerStateConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            switch ((MediaPlaybackState)value) {
                case MediaPlaybackState.Playing:
                    return new SymbolIcon(Symbol.Pause);
                default:
                    return new SymbolIcon(Symbol.Play);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
