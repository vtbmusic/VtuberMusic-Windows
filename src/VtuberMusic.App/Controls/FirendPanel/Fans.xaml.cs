using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.FirendPanel;
public sealed partial class Fans : UserControl {
    public DependencyProperty UserIdProperty =
        DependencyProperty.Register("UserId", typeof(string), typeof(Fans), new PropertyMetadata(null));

    public string UserId {
        get => (string)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }

    public Fans() {
        InitializeComponent();
    }

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) => ViewModel.NavigationService.Navigate<ProfilePage>(new ProfilePageArg { Profile = e.ClickedItem as Profile });
}
