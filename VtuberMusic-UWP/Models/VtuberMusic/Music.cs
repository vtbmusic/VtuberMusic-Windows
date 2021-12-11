using PropertyChanged;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class Music : INotifyPropertyChanged {
        public string id { get; set; }
        public string name { get; set; }
        public object alias { get; set; }
        public string picUrl { get; set; }
        public string vrcUrl { get; set; }
        public int time { get; set; }
        public float duration { get; set; }
        public Artist[] artists { get; set; }
        public MusicStatis statis { get; set; }

        public bool IsPlaying {
            get {
                return this == App.Player.NowPlayingMusic;
            }
        }

        public bool like { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [AddINotifyPropertyChangedInterface]
    public class MusicStatis : INotifyPropertyChanged {
        public int playCount { get; set; }
        public int commentCount { get; set; }
        public int likeCount { get; set; }
        public int shareCount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
