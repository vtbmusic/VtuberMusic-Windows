using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.FriendsPanel {
    public class FollowersViewModel : FriendsPanelViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public ObservableCollection<Profile> Followers = new ObservableCollection<Profile>();

        public FollowersViewModel() : base() {
            LoadCommand = new AsyncRelayCommand<string>(LoadDataAsync);
        }

        public override async Task LoadDataAsync(string userId) {
            var result = await _vtuberMusicService.GetFollows(userId, 100);

            Followers.Clear();
            foreach (var item in result.Data) {
                Followers.Add(item);
            }
        }
    }
}
