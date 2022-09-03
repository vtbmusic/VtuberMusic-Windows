using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Pages;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class LoginPage : Page {
    private readonly LoginPageViewModel ViewModel = Ioc.Default.GetRequiredService<LoginPageViewModel>();

    public LoginPage() {
        InitializeComponent();
    }
}
