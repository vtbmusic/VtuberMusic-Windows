using CommunityToolkit.Mvvm.ComponentModel;

namespace VtuberMusic.App.ViewModels.Pages;

public partial class NoticePageViewModel : ObservableObject {
    [ObservableProperty]
    private bool isSystemNoticeShow = false;
    [ObservableProperty]
    private bool isReplyNoticeShow = false;
}