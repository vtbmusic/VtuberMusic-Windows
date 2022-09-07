using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VtuberMusic.App.Messages;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class CreatePlaylistDialogViewModel : ObservableValidator {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    [Required]
    private string playlistName;

    [ObservableProperty]
    private bool isPrivacy;

    public CreatePlaylistDialogViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task CreatePlaylist() {
        ValidateAllProperties();

        if (!this.HasErrors) {
            string isPrivacyArg = null;
            if (this.IsPrivacy)
                isPrivacyArg = "true";

            await _vtuberMusicService.CreatePlaylist(this.PlaylistName, isPrivacyArg);
            WeakReferenceMessenger.Default.Send(new UserPlaylistsChangedMessage());
        }
    }
}
