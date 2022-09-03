using CommunityToolkit.Mvvm.Messaging.Messages;
using Windows.Media.Playback;

namespace VtuberMusic.AppCore.Messages;
public class PlaybackStatueChangedMessage : ValueChangedMessage<MediaPlaybackState> {
    public PlaybackStatueChangedMessage(MediaPlaybackState state) : base(state) {

    }
}
