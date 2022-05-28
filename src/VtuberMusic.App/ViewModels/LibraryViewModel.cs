using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels {
    public class LibraryViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public ObservableCollection<Music> PersonalizedMusic = new ObservableCollection<Music>();
        public ObservableCollection<Playlist> CreatePlaylist = new ObservableCollection<Playlist>();
        public ObservableCollection<Playlist> SubPlaylist = new ObservableCollection<Playlist>();

        public Playlist DailyRecommenderPlaylist { get => dailyRecommenderPlaylist; set => SetProperty(ref dailyRecommenderPlaylist, value); }
        private Playlist dailyRecommenderPlaylist;
        public Playlist FavouritePlaylist { get => favouritePlaylist; set => SetProperty(ref favouritePlaylist, value); }
        private Playlist favouritePlaylist;

        public Music PersonalizedFirstMusic { get => personalizedFirstMusic; set => SetProperty(ref personalizedFirstMusic, value); }
        private Music personalizedFirstMusic;

        public LibraryViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            try {
                var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
                var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();
                var favouritePlaylistResponse = await _vtuberMusicService.GetFavouriteMusicsPlaylist("song");
                var subPlaylistResponse = await _vtuberMusicService.GetSubPlaylist();
                var createPlaylistResponse = await _vtuberMusicService.GetCreatePlaylist();

                PersonalizedMusic.Clear();
                foreach (var item in personalizedMusicResponse.Data) PersonalizedMusic.Add(item);

                SubPlaylist.Clear();
                foreach (var item in subPlaylistResponse.Data) SubPlaylist.Add(item);

                CreatePlaylist.Clear();
                foreach (var item in createPlaylistResponse.Data) CreatePlaylist.Add(item);

                PersonalizedFirstMusic = PersonalizedMusic.FirstOrDefault();
                DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
                FavouritePlaylist = favouritePlaylistResponse.Data.playlist;
            } catch (Exception ex) {
            }
        }
    }
}
