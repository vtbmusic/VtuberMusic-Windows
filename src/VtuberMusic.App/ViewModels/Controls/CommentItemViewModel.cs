using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public partial class CommentItemViewModel : ObservableObject {
    [ObservableProperty]
    private CommentContentType type;

    [ObservableProperty]
    private Comment comment;

    private readonly IVtuberMusicService _vtuberMusicService;

    public CommentItemViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public void NavigateToProfile(Profile profile) =>
        NavigationHelper.Navigate<ProfilePage>(new ProfilePageArg { Profile = profile });

    [RelayCommand]
    public async Task DeleteComment() {
        await _vtuberMusicService.DeleteComment(this.Comment.id, this.Type);
    }
}
