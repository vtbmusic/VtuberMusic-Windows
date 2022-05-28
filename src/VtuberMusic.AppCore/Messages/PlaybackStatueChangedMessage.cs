using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackStatueChangedMessage : ValueChangedMessage<MediaPlaybackState> {
        public PlaybackStatueChangedMessage(MediaPlaybackState state) : base(state) {

        }
    }
}
