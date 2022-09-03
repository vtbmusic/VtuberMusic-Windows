using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Enums;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackPlaylistModeChangedMessage : ValueChangedMessage<PlaylistPlayMode> {
        public PlaybackPlaylistModeChangedMessage(PlaylistPlayMode mode) : base(mode) {

        }
    }
}
