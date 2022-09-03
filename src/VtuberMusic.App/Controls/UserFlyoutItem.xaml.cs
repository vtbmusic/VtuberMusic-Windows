using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.ViewModels;
using VtuberMusic.AppCore.Enums;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class UserFlyout : UserControl {
    private readonly UserFlyoutViewModel ViewModel = Ioc.Default.GetRequiredService<UserFlyoutViewModel>();

    public UserFlyout() {
        InitializeComponent();
    }

    private void SubCountButton_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<Friends>(new FriendsPageArg { Profile = ViewModel.Profile, Type = FriendsPageType.Follwers });

    private void FansCountButton_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<Friends>(new FriendsPageArg { Profile = ViewModel.Profile, Type = FriendsPageType.Fans });
}
