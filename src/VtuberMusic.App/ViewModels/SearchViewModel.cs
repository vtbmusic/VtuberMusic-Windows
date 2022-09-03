using CommunityToolkit.Mvvm.ComponentModel;

namespace VtuberMusic.App.ViewModels;
public partial class SearchViewModel : ObservableObject {
    [ObservableProperty]
    private string keyword;

    [ObservableProperty]
    private bool isSearchUser;
    [ObservableProperty]
    private bool isSearchMusic;
    [ObservableProperty]
    private bool isSearchPlaylist;
    [ObservableProperty]
    private bool isSearchArtist;
}
