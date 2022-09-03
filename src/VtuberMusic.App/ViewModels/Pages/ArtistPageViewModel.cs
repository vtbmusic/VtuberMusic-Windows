using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class ArtistPageViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    public ArtistPageViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private Artist artist;
    [ObservableProperty]
    private ObservableCollection<Music> musics = new();

    public ArtistPageViewModel() {
    }

    [RelayCommand]
    private async Task Load() {
        this.Artist = (await _vtuberMusicService.GetArtistDetail(this.Artist.id)).Data;
        var musics = await _vtuberMusicService.GetArtistMusics(this.Artist.id);

        this.Musics.Clear();
        foreach (var item in musics.Data) {
            this.Musics.Add(item);
        }
    }

    [RelayCommand]
    public void CopyLink(object arg) {
        DataPackage data = new();
        if (arg is Music) {
            data.SetText($"https://vtbmusic.com/song?id={(arg as Music).id}");
        } else if (arg is Artist) {
            data.SetText($"https://vtbmusic.com/vtuber?id={(arg as Artist).id}");
        } else if (arg is Playlist) {
            data.SetText($"https://vtbmusic.com/songlist?id={(arg as Playlist).id}");
        }

        Clipboard.SetContent(data);
    }

    [RelayCommand]
    public void Share(object arg) {
        if (arg is Music) {
            ShareHelper.ShareMusic(arg as Music);
        } else if (arg is Artist) {
            ShareHelper.ShareArtist(arg as Artist);
        } else if (arg is Playlist) {
            ShareHelper.SharePlaylist(arg as Playlist);
        }
    }
}
