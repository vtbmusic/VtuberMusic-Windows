using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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
