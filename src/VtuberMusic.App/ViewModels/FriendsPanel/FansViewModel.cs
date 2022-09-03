using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.FriendsPanel;
public class FansViewModel : FriendsPanelViewModel {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

    public ObservableCollection<Profile> Fans = new();

    public FansViewModel() : base() {
        LoadCommand = new AsyncRelayCommand<string>(LoadDataAsync);
    }

    public override async Task LoadDataAsync(string userId) {
        var result = await _vtuberMusicService.GetFans(userId, 100);

        Fans.Clear();
        foreach (var item in result.Data) {
            Fans.Add(item);
        }
    }
}
