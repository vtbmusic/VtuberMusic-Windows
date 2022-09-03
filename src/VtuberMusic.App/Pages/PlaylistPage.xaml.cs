using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.PageArgs;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class PlaylistPage : Page {
    public PlaylistPage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        var arg = e.Parameter as PlaylistPageArg;
        ViewModel.Playlist = arg.Playlist;
        ViewModel.PlaylistType = arg.PlaylistType;
    }
}
