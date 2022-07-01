using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackMusicChangedMessage : ValueChangedMessage<Music> {
        public PlaybackMusicChangedMessage(Music music) : base(music) {

        }
    }
}
