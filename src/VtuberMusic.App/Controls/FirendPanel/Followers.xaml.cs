using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.ViewModels.FriendsPanel;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.FirendPanel;
public sealed partial class Followers : UserControl {
    public DependencyProperty UserIdProperty =
        DependencyProperty.Register("UserId", typeof(string), typeof(Followers), new PropertyMetadata(null));

    private readonly FollowersViewModel ViewModel = Ioc.Default.GetRequiredService<FollowersViewModel>();

    public string UserId {
        get => (string)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }

    public Followers() {
        InitializeComponent();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) => NavigationHelper.Navigate<ProfilePage>(new ProfilePageArg { Profile = e.ClickedItem as Profile });
}
