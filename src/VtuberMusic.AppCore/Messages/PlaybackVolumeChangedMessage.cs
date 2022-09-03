using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VtuberMusic.AppCore.Messages;
public class PlaybackVolumeChangedMessage : ValueChangedMessage<double> {
    public PlaybackVolumeChangedMessage(double volume) : base(volume) {

    }
}
