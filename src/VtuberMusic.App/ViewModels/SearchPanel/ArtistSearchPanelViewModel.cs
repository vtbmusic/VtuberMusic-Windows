using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel {
    public partial class ArtistSearchPanelViewModel : ObservableObject {
        private readonly IVtuberMusicService _vtuberMusicService;

        public ArtistSearchPanelViewModel(IVtuberMusicService vtuberMusicService) {
            _vtuberMusicService = vtuberMusicService;
        }

        [ObservableProperty]
        private string keyword;

        [ObservableProperty]
        private ObservableCollection<Artist> artists = new();

        [RelayCommand]
        public async Task Search() {
            this.Artists.Clear();
            var data = await _vtuberMusicService.SearchArtists(this.Keyword);

            foreach (var item in data.Data) {
                this.Artists.Add(item);
            }
        }
    }
}
