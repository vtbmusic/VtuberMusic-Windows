using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.NoticePanel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.NoitcePanel;
public sealed partial class ReplyNoticePanel : UserControl {
    private readonly ReplyNoticePanelViewModel ViewModel = Ioc.Default.GetRequiredService<ReplyNoticePanelViewModel>();

    public ReplyNoticePanel() {
        this.InitializeComponent();
    }
}
