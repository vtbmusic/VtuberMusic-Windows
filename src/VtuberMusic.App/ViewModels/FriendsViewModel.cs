using CommunityToolkit.Mvvm.ComponentModel;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.ViewModels;
public partial class FriendsViewModel : ObservableObject {
    [ObservableProperty]
    private Profile profile;

    [ObservableProperty]
    private bool isFansShow;

    [ObservableProperty]
    private bool isFollwerdsShow;
}
