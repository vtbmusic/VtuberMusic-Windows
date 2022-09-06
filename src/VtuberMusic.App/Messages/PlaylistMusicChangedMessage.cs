using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VtuberMusic.App.Messages;
public class PlaylistMusicChangedMessage : ValueChangedMessage<string> {
    public PlaylistMusicChangedMessage(string value) : base(value) {
    }
}
