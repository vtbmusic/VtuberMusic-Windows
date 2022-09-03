using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel;
public partial class PlaylistSearchPanelViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    public PlaylistSearchPanelViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private string keyword;

    [ObservableProperty]
    private ObservableCollection<Playlist> playlists = new();

    [RelayCommand]
    public async Task Search() {
        this.Playlists.Clear();
        var data = await _vtuberMusicService.SearchPlaylist(this.Keyword);

        foreach (var item in data.Data) {
            this.Playlists.Add(item);
        }
    }
}
