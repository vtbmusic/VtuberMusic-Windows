using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;

namespace VtuberMusic.AppCore.Messages {
    public class PlaybackMusicChangedMessage : ValueChangedMessage<Music> {
        public PlaybackMusicChangedMessage(Music music) : base(music) {

        }
    }
}
