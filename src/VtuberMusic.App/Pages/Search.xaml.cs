using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.ViewModels.Pages;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class Search : Page {
    private readonly SearchViewModel ViewModel = Ioc.Default.GetRequiredService<SearchViewModel>();

    public Search() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        ViewModel.Keyword = (e.Parameter as SearchPageArg).Keyword;
    }

    private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
        hideSearch();
        if ((Nav.SelectedItem as NavigationViewItem) == MusicItem) {
            ViewModel.IsSearchMusic = true;
        } else if ((Nav.SelectedItem as NavigationViewItem) == ArtistItem) {
            ViewModel.IsSearchArtist = true;
        } else if ((Nav.SelectedItem as NavigationViewItem) == PlaylistItem) {
            ViewModel.IsSearchPlaylist = true;
        } else if ((Nav.SelectedItem as NavigationViewItem) == UserItem) {
            ViewModel.IsSearchUser = true;
        }
    }

    private void hideSearch() {
        ViewModel.IsSearchArtist = false;
        ViewModel.IsSearchMusic = false;
        ViewModel.IsSearchPlaylist = false;
        ViewModel.IsSearchUser = false;
    }
}
