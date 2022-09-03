using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.ViewModels {
    public partial class AppViewModel {
        public IRelayCommand<Music> SetMusicCommand { get; }
        public IRelayCommand<Music> SetNextMusicCommand { get; }
        public IRelayCommand<IEnumerable<Music>> SetCollectionCommand { get; }

        public IRelayCommand<Playlist> NavigateToPlaylistCommand { get; }
        public IRelayCommand<Artist> NavigateToArtistCommand { get; }
        public IRelayCommand<Profile> NavigateToProfileCommand { get; }
        public IRelayCommand<string> NavigateToSearchCommand { get; }

        public IRelayCommand<object> CopyLinkCommand { get; }
        public IRelayCommand<object> ShareCommand { get; }
    }
}
