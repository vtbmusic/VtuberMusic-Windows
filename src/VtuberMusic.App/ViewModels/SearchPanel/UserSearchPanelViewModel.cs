using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.SearchPanel {
    public partial class UserSearchPanelViewModel : ObservableObject {
        private readonly IVtuberMusicService _vtuberMusicService;

        public UserSearchPanelViewModel(IVtuberMusicService vtuberMusicService) {
            _vtuberMusicService = vtuberMusicService;
        }

        [ObservableProperty]
        private string keyword;

        [ObservableProperty]
        private ObservableCollection<Profile> users = new();

        [RelayCommand]
        public async Task Search() {
            this.Users.Clear();
            var data = await _vtuberMusicService.SearchUser(this.Keyword);

            foreach (var item in data.Data) {
                this.Users.Add(item);
            }
        }
    }
}
