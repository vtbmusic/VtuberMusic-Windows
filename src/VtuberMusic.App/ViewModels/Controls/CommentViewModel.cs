using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class CommentViewModel : ObservableValidator {
    private readonly IVtuberMusicService _vtuberMusicService;
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private Music music;

    [ObservableProperty]
    private CommentContentType type;

    [ObservableProperty]
    [Required]
    private string content;

    [ObservableProperty]
    private ObservableCollection<Comment> comments = new ObservableCollection<Comment>();

    [ObservableProperty]
    private string nickname;

    public CommentViewModel(IVtuberMusicService vtuberMusicService, IAuthorizationService authorizationService) {
        _vtuberMusicService = vtuberMusicService;
        _authorizationService = authorizationService;
    }

    [RelayCommand]
    public async Task Load() {
        this.Nickname = _authorizationService.Profile.nickname;

        ApiResponse<CommentResponse> response = null;
        switch (this.Type) {
            case CommentContentType.song:
                response = await _vtuberMusicService.GetMusicComments(this.Music.id);
                break;
            case CommentContentType.playlist:
                response = await _vtuberMusicService.GetPlaylistComments(this.Playlist.id);
                break;
        }

        if (response != null) {
            this.Comments.Clear();
            Array.ForEach(response.Data.comments, (item) => this.Comments.Add(item));
        }
    }

    [RelayCommand]
    public async Task Send() {
        ValidateAllProperties();
        if (this.HasErrors)
            return;

        switch (this.Type) {
            case CommentContentType.song:
                await _vtuberMusicService.AddComment(this.Music.id, CommentContentType.song, this.Content);
                break;
            case CommentContentType.playlist:
                await _vtuberMusicService.AddComment(this.Playlist.id, CommentContentType.playlist, this.Content);
                break;
        }

        this.Content = "";
        await Load();
    }

    [RelayCommand]
    public Task Reply(string commentId) {
        return Task.CompletedTask;
    }
}
