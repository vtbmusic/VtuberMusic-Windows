using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class DiscoverViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    public DiscoverViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private ObservableCollection<Music> newMusics = new ObservableCollection<Music>();
    [ObservableProperty]
    private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
    [ObservableProperty]
    private ObservableCollection<Music> personalizedMusic = new ObservableCollection<Music>();
    [ObservableProperty]
    private ObservableCollection<Artist> artists = new ObservableCollection<Artist>();

    public DateTimeOffset Today = DateTimeOffset.Now;

    [ObservableProperty]
    private Playlist dailyRecommenderPlaylist;

    [ObservableProperty]
    private Music personalizedFirstMusic;

    [RelayCommand]
    private async Task Load() {
        try {
            var newMusicResponse = await _vtuberMusicService.GetNewMusic();
            var playlistResponse = await _vtuberMusicService.GetPlaylists();
            var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
            var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();
            var artistsResponse = await _vtuberMusicService.GetArtists();

            this.NewMusics.Clear();
            Array.ForEach(newMusicResponse.Data, (item) => this.NewMusics.Add(item));

            this.Playlists.Clear();
            Array.ForEach(playlistResponse.Data, (item) => this.Playlists.Add(item));

            this.PersonalizedMusic.Clear();
            Array.ForEach(personalizedMusicResponse.Data, (item) => this.PersonalizedMusic.Add(item));

            this.Artists.Clear();
            Array.ForEach(artistsResponse.Data, (item) => this.Artists.Add(item));

            this.PersonalizedFirstMusic = this.PersonalizedMusic.FirstOrDefault();
            this.DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
        } catch (Exception ex) {
            Debug.Write(ex);
        }
    }
}
