using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Messages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class ConfirmDeletePlaylistDialogViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private Playlist playlistDelete;

    public ConfirmDeletePlaylistDialogViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task ConfirmDelete() {
        await _vtuberMusicService.DeletePlaylist(this.PlaylistDelete.id);
        WeakReferenceMessenger.Default.Send(new UserPlaylistsChangedMessage());
    }
}
