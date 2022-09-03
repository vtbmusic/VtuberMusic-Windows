using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.ViewModels.Pages;
using VtuberMusic.AppCore.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class Friends : Page {
    private readonly FriendsViewModel ViewModel = Ioc.Default.GetRequiredService<FriendsViewModel>();

    public Friends() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        if (e.Parameter is FriendsPageArg arg) {
            ViewModel.Profile = arg.Profile;
            Nav.SelectedItem = arg.Type switch {
                FriendsPageType.Fans => FansItem,
                FriendsPageType.Follwers => SubItem,
                _ => SubItem,
            };
        }
    }

    private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
        hideAll();

        if ((args.SelectedItem as NavigationViewItem) == FansItem) {
            ViewModel.IsFansShow = true;
        } else if ((args.SelectedItem as NavigationViewItem) == SubItem) {
            ViewModel.IsFollwerdsShow = true;
        }
    }

    private void hideAll() {
        ViewModel.IsFansShow = false;
        ViewModel.IsFollwerdsShow = false;
    }
}
