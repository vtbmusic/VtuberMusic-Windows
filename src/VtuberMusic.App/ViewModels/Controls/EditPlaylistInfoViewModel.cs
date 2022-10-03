using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Messages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class EditPlaylistInfoDialogViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private Playlist playlist;

    public EditPlaylistInfoDialogViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task EditPlaylist() {
        await _vtuberMusicService.UpdatePlaylistInfo(this.Playlist.id, this.Playlist.name, this.Playlist.description, new string[] { "tag" });
        WeakReferenceMessenger.Default.Send(new UserPlaylistsChangedMessage());
    }
}
