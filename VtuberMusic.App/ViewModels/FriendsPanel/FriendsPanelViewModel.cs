using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace VtuberMusic.App.ViewModels.FriendsPanel {
    public class FriendsPanelViewModel : AppViewModel {
        public AsyncRelayCommand<string> LoadCommand;

        public virtual Task LoadDataAsync(string userId) { return Task.CompletedTask; }
    }
}
