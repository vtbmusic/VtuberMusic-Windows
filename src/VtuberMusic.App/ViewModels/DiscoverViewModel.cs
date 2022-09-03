using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public class DiscoverViewModel : AppViewModel {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

    public IAsyncRelayCommand LoadCommand { get; }

    public ObservableCollection<Music> NewMusics = new();
    public ObservableCollection<Banner> Banners = new();
    public ObservableCollection<Playlist> Playlists = new();
    public ObservableCollection<Music> PersonalizedMusic = new();

    public DateTimeOffset Today = DateTimeOffset.Now;
    public Playlist DailyRecommenderPlaylist { get => dailyRecommenderPlaylist; set => SetProperty(ref dailyRecommenderPlaylist, value); }
    private Playlist dailyRecommenderPlaylist;

    public Music PersonalizedFirstMusic { get => personalizedFirstMusic; set => SetProperty(ref personalizedFirstMusic, value); }
    private Music personalizedFirstMusic;

    public DiscoverViewModel() {
        this.LoadCommand = new AsyncRelayCommand(LoadDataAsync);
    }

    private async Task LoadDataAsync() {
        try {
            var newMusicResponse = await _vtuberMusicService.GetNewMusic();
            var bannerResponse = await _vtuberMusicService.GetBanner();
            var playlistResponse = await _vtuberMusicService.GetPlaylists();
            var dailyRecommenderResponse = await _vtuberMusicService.GetDailyPersonalizedMusic();
            var personalizedMusicResponse = await _vtuberMusicService.GetPersonalizedMusic();

            NewMusics.Clear();
            foreach (var item in newMusicResponse.Data) {
                NewMusics.Add(item);
            }

            Banners.Clear();
            foreach (var item in bannerResponse.Data) {
                Banners.Add(item);
            }

            Playlists.Clear();
            foreach (var item in playlistResponse.Data) {
                Playlists.Add(item);
            }

            Playlists.Clear();
            foreach (var item in playlistResponse.Data) {
                Playlists.Add(item);
            }

            PersonalizedMusic.Clear();
            foreach (var item in personalizedMusicResponse.Data) {
                PersonalizedMusic.Add(item);
            }

            this.PersonalizedFirstMusic = PersonalizedMusic.FirstOrDefault();
            this.DailyRecommenderPlaylist = dailyRecommenderResponse.Data.playlist;
        } catch (Exception ex) {
            Debug.Write(ex);
        }
    }
}
