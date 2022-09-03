using VtuberMusic.Core.Models;

namespace VtuberMusic.App.ViewModels {
    public class FriendsViewModel : AppViewModel {
        public Profile Profile { get => profile; set => SetProperty(ref profile, value); }
        private Profile profile;

        public bool IsFansShow { get => isFansShow; set => SetProperty(ref isFansShow, value); }
        private bool isFansShow;

        public bool IsFollwerdsShow { get => isFollwerdsShow; set => SetProperty(ref isFollwerdsShow, value); }

        private bool isFollwerdsShow;
    }
}
