using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Pages;
using VtuberMusic.App.Services;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
    private readonly WindowBackdropHelper _backdropHelper;
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetRequiredService<IAuthorizationService>();
    private readonly IAppCenterReleasesService _appCenterReleasesService = Ioc.Default.GetRequiredService<IAppCenterReleasesService>();

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

        checkUpdate();
    }

    private async void checkUpdate() {
        try {
            var releases = await _appCenterReleasesService.GetReleasesAsync();
            if (releases.Count() != 0) {
                var release = await _appCenterReleasesService.GetReleaseAsync(releases.First().id);
                var version = Package.Current.Id.Version;

                if (release.version != $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}") {
                    var dialog = new ContentDialog() {
                        XamlRoot = this.Content.XamlRoot,
                        Title = "新版本可用",
                        PrimaryButtonText = "前往外部链接更新",
                        DefaultButton = ContentDialogButton.Primary,
                        Content = new UpdateAvailableDialog() { Releases = release }
                    };

                    if (await dialog.ShowAsync() == ContentDialogResult.Primary) {
                        await Launcher.LaunchUriAsync(new Uri("https://install.appcenter.ms/users/vtubermusic-misaka-l/apps/vtubermusic-uwp/distribution_groups/users"));
                    }
                }
            }
        } catch { }
    }
}
