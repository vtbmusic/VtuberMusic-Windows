using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace VtuberMusic_UWP.Models.VtuberMusic {
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

        private bool _like { get; set; }
        public bool like {
            get { return this._like; }
            set {
                this._like = value;
                this.NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            await App.RootFrame.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
    }

    public class MusicStatis {
        public int playCount { get; set; }
        public int commentCount { get; set; }
        public int likeCount { get; set; }
        public int shareCount { get; set; }
    }
}
