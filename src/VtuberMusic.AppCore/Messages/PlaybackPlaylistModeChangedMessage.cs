using CommunityToolkit.Mvvm.Messaging.Messages;
using VtuberMusic.AppCore.Enums;

namespace VtuberMusic.AppCore.Messages;
public class PlaybackPlaylistModeChangedMessage : ValueChangedMessage<PlaylistPlayMode> {
    public PlaybackPlaylistModeChangedMessage(PlaylistPlayMode mode) : base(mode) {

    }
}
