using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NoticePage : Page {
    private readonly NoticePageViewModel ViewModel = Ioc.Default.GetRequiredService<NoticePageViewModel>();

    public NoticePage() {
        this.InitializeComponent();
    }

    private void NavigationView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        hideAll();
        switch ((args.SelectedItem as NavigationViewItem)?.Tag as string) {
            case "Reply":
                ViewModel.IsReplyNoticeShow = true;
                break;
            case "System":
                ViewModel.IsSystemNoticeShow = true;
                break;
        }
    }

    private void hideAll() {
        ViewModel.IsReplyNoticeShow = false;
        ViewModel.IsSystemNoticeShow = false;
    }
}
