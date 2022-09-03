using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class UserFlyout : UserControl {
    public UserFlyout() {
        InitializeComponent();
    }

    private void SubCountButton_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<Firends>(new FirendsPageArg { Profile = ViewModel.Profile, Type = FirendsPageType.Follwers });

    private void FansCountButton_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<Firends>(new FirendsPageArg { Profile = ViewModel.Profile, Type = FirendsPageType.Fans });
}
