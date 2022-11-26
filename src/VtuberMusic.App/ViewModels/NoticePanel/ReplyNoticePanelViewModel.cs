using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.NoticePanel;
public partial class ReplyNoticePanelViewModel : ObservableObject {
    private readonly IVtuberMusicService _vtuberMusicService;

    [ObservableProperty]
    private ObservableCollection<CommentNotice> commentNotices = new();

    public ReplyNoticePanelViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [RelayCommand]
    public async Task Load() {
        commentNotices.Clear();

        var response = await _vtuberMusicService.GetReplyNotices();
        foreach (var notice in response.Data) {
            commentNotices.Add(notice);
        }
    }
}
