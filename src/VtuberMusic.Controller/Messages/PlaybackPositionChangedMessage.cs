using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackPositionChangedMessage : ValueChangedMessage<PlaybackPositionChangedMessageData> {
        public PlaybackPositionChangedMessage(PlaybackPositionChangedMessageData data) : base(data) {

        }
    }

    public class PlaybackPositionChangedMessageData {
        public TimeSpan Position { get; set; }
        public TimeSpan Duration { get; set; }

        public PlaybackPositionChangedMessageData(TimeSpan position, TimeSpan duration) {
            Position = position;
            Duration = duration;
        }
    }
}
