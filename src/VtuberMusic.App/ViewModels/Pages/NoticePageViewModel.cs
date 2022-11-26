using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;

public partial class NoticePageViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private bool isSystemNoticeShow = false;
    [ObservableProperty]
    private bool isReplyNoticeShow = false;

    [ObservableProperty]
    private MessageCount messageCount;

    public NoticePageViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task Load() {
        var response = await _vtuberMusicService.GetNoticeCount();
        this.MessageCount = response.Data;
    }
}