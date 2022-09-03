using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.FriendsPanel;
public partial class FollowersViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    public FollowersViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private ObservableCollection<Profile> followers = new ObservableCollection<Profile>();

    public FollowersViewModel() {
    }

    [RelayCommand]
    public async Task Load(string userId) {
        var result = await _vtuberMusicService.GetFollows(userId, 100);

        this.Followers.Clear();
        foreach (var item in result.Data) {
            this.Followers.Add(item);
        }
    }
}
