using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.NoticePanel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.NoitcePanel;
public sealed partial class SystemNoticePanel : UserControl {
    private readonly SystemNoticePanelViewModel ViewModel = Ioc.Default.GetRequiredService<SystemNoticePanelViewModel>();

    public SystemNoticePanel() {
        this.InitializeComponent();
    }
}
