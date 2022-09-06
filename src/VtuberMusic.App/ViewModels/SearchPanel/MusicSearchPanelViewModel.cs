using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel;
public partial class MusicSearchPanelViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    public MusicSearchPanelViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private string keyword;

    [ObservableProperty]
    private ObservableCollection<Music> musics = new();

    [RelayCommand]
    public async Task Search() {
        this.Musics.Clear();
        var data = await _vtuberMusicService.SearchMusic(this.Keyword);

        foreach (var item in data.Data) {
            this.Musics.Add(item);
        }
    }
}
