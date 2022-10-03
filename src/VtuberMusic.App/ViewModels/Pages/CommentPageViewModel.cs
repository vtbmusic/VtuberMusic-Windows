using CommunityToolkit.Mvvm.ComponentModel;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class CommentPageViewModel : ObservableObject {
    [ObservableProperty]
    private CommentContentType type;

    [ObservableProperty]
    private Playlist playlist;

    [ObservableProperty]
    private Music music;
}
