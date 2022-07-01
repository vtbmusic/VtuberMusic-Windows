using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels {
    public class ProfilePageViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public Profile Profile { get => profile; set => SetProperty(ref profile, value); }
        private Profile profile;

        public ProfilePageViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            Profile = (await _vtuberMusicService.GetProfile(Profile.userId)).Data.profile;
        }
    }
}
