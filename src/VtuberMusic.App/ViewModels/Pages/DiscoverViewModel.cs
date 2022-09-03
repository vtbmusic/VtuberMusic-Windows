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
    private ObservableCollection<Banner> banners = new ObservableCollection<Banner>();
    [ObservableProperty]
    private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
    [ObservableProperty]
    private ObservableCollection<Music> personalizedMusic = new ObservableCollection<Music>();

    public DateTimeOffset Today = DateTimeOffset.Now;

    [ObservableProperty]
    private Playlist dailyRecommenderPlaylist;

    [ObservableProperty]
    private Music personalizedFirstMusic;

    [RelayCommand]
    private async Task Load() {
        try {
            var newMusicResponse = await _vtuberMusicService.GetNewMusic();
            var bannerResponse = await _vtuberMusicService.GetBanner();
            var playlistResponse = await _vtuberMusicService.GetPlaylists();
            var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
            var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();

            this.NewMusics.Clear();
            foreach (var item in newMusicResponse.Data) {
                this.NewMusics.Add(item);
            }

            this.Banners.Clear();
            foreach (var item in bannerResponse.Data) {
                this.Banners.Add(item);
            }

            this.Playlists.Clear();
            foreach (var item in playlistResponse.Data) {
                this.Playlists.Add(item);
            }

            this.Playlists.Clear();
            foreach (var item in playlistResponse.Data) {
                this.Playlists.Add(item);
            }

            this.PersonalizedMusic.Clear();
            foreach (var item in personalizedMusicResponse.Data) {
                this.PersonalizedMusic.Add(item);
            }

            this.PersonalizedFirstMusic = this.PersonalizedMusic.FirstOrDefault();
            this.DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
        } catch (Exception ex) {
            Debug.Write(ex);
        }
    }
}
