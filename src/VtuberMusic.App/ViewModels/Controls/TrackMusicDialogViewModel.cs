using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class TrackMusicDialogViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private Playlist favoriteMusicPlaylist;
    [ObservableProperty]
    private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();

    public TrackMusicDialogViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task Load() {
        this.FavoriteMusicPlaylist = (await _vtuberMusicService.GetFavouriteMusicsPlaylist()).Data.playlist;

        var playlistsData = (await _vtuberMusicService.GetCreatePlaylist()).Data;
        Array.ForEach(playlistsData, (item) => this.Playlists.Add(item));
    }

    public async Task TrackMusicAsync(string playlistId, string[] musicId, bool isFavoritePlaylist = false) {
        if (isFavoritePlaylist) {
            foreach (var item in musicId) {
                await _vtuberMusicService.LikeMusic(item);
            }
        } else {
            await _vtuberMusicService.TrackMusic(playlistId, TrackType.add, musicId);
        }
    }
}
