using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel {
    public class PlaylistSearchPanelViewModel : SearchPanelViewModel {
        private IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
        public ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();

        protected async override Task LoadResultAsync() {
            Playlists.Clear();
            var data = await _vtuberMusicService.SearchPlaylist(Keyword);

            foreach (var item in data.Data) {
                Playlists.Add(item);
            }
        }
    }
}
