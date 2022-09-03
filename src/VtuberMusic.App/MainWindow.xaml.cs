using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
    private readonly WindowBackdropHelper _backdropHelper;
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetRequiredService<IAuthorizationService>();

    public MainWindow() {
        InitializeComponent();
        _backdropHelper = new WindowBackdropHelper(this);
        _backdropHelper.TrySetMicaBackdrop();

        this.Title = "VtuberMusic";
        this.ExtendsContentIntoTitleBar = true;

        App.MainWindow = this;
        App.RootFrame = RootFrame;
        DispatcherHelper.Init(this.DispatcherQueue);

        if (!_authorizationService.IsLogin) {
            RootFrame.Navigate(typeof(LoginPage));
        } else {
            RootFrame.Navigate(typeof(MainPage));
        }
    }
}
