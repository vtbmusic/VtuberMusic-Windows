namespace VtuberMusic.App.ViewModels;
public class SearchViewModel : AppViewModel {
    public string Keyword { get => keyword; set => SetProperty(ref keyword, value); }
    private string keyword;

    public bool IsSearchMusic { get => isSearchMusic; set => SetProperty(ref isSearchMusic, value); }
    public bool IsSearchArtist { get => isSearchArtist; set => SetProperty(ref isSearchArtist, value); }
    public bool IsSearchPlaylist { get => isSearchPlaylist; set => SetProperty(ref isSearchPlaylist, value); }
    public bool IsSearchUser { get => isSearchUser; set => SetProperty(ref isSearchUser, value); }

    private bool isSearchUser;
    private bool isSearchMusic;
    private bool isSearchPlaylist;
    private bool isSearchArtist;
}
