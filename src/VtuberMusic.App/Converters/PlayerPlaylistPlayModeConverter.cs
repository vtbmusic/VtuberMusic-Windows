using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Enums;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace VtuberMusic.App.Converters {
    public class PlayerPlaylistPlayModeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            switch ((PlaylistPlayMode)value) {
                case PlaylistPlayMode.PlaylistRepeat:
                    return new SymbolIcon(Symbol.RepeatAll);
                case PlaylistPlayMode.SingleRepeat:
                    return new SymbolIcon(Symbol.RepeatOne);
                case PlaylistPlayMode.Shuffle:
                    return new SymbolIcon(Symbol.Shuffle);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
