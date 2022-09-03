﻿using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels {
    public class ArtistPageViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public Artist Artist { get => artist; set => SetProperty(ref artist, value); }
        public ObservableCollection<Music> Musics = new ObservableCollection<Music>();

        private Artist artist;

        public ArtistPageViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            Artist = (await _vtuberMusicService.GetArtistDetail(Artist.id)).Data;
            var musics = await _vtuberMusicService.GetArtistMusics(Artist.id);

            Musics.Clear();
            foreach (var item in musics.Data) Musics.Add(item);
        }
    }
}
