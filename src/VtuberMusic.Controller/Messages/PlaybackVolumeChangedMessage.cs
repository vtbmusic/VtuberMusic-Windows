using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackVolumeChangedMessage : ValueChangedMessage<double> {
        public PlaybackVolumeChangedMessage(double volume) : base(volume) {

        }
    }
}
