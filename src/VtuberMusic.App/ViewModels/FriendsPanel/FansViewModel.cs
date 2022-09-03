using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.FriendsPanel;
public partial class FansViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

    [ObservableProperty]
    private ObservableCollection<Profile> fans = new ObservableCollection<Profile>();

    public FansViewModel() {
    }

    [RelayCommand]
    public async Task Load(string userId) {
        var result = await _vtuberMusicService.GetFans(userId, 100);

        this.Fans.Clear();
        foreach (var item in result.Data) {
            this.Fans.Add(item);
        }
    }
}
