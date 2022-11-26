using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.NoticePanel;

public partial class SystemNoticePanelViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private ObservableCollection<Notice> _notices = new();

    public SystemNoticePanelViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task Load() {
        _notices.Clear();

        await _vtuberMusicService.ReadNotice(NoticeType.System);
        var response = await _vtuberMusicService.GetSystemNotices();
        foreach (var notice in response.Data) {
            _notices.Add(notice);
        }
    }
}