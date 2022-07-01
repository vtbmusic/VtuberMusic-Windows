using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel {
    public class MusicSearchPanelViewModel : SearchPanelViewModel {
        private IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
        public ObservableCollection<Music> Musics = new ObservableCollection<Music>();

        protected async override Task LoadResultAsync() {
            Musics.Clear();
            var data = await _vtuberMusicService.SearchMusic(Keyword);

            foreach (var item in data.Data) {
                Musics.Add(item);
            }
        }
    }
}
