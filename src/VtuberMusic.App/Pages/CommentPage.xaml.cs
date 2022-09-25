using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CommentPage : Page {
    private readonly CommentPageViewModel ViewModel = Ioc.Default.GetRequiredService<CommentPageViewModel>();

    public CommentPage() {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        var arg = (CommentPageArg)e.Parameter;
        ViewModel.Type = arg.Type;
        ViewModel.Music = arg.Music;
        ViewModel.Playlist = arg.Playlist;
    }
}
