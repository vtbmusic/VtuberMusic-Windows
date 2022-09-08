using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.ViewModels.Pages;
using VtuberMusic.AppCore.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class ProfilePage : Page {
    private readonly ProfilePageViewModel ViewModel = Ioc.Default.GetRequiredService<ProfilePageViewModel>();

    public ProfilePage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);
        ViewModel.Profile = (e.Parameter as ProfilePageArg).Profile;
    }

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        ViewModel.IsActive = true;

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        ViewModel.IsActive = false;

    private void FansButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        NavigationHelper.Navigate<Friends>(new FriendsPageArg { Profile = ViewModel.Profile, Type = FriendsPageType.Fans });
    private void FollowButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        NavigationHelper.Navigate<Friends>(new FriendsPageArg { Profile = ViewModel.Profile, Type = FriendsPageType.Follwers });
}
