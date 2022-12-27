using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Refit;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Services;
using VtuberMusic.App.ViewModels;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.App.ViewModels.Controls.DataItem;
using VtuberMusic.App.ViewModels.FriendsPanel;
using VtuberMusic.App.ViewModels.Lyric;
using VtuberMusic.App.ViewModels.NoticePanel;
using VtuberMusic.App.ViewModels.Pages;
using VtuberMusic.App.ViewModels.SearchPanel;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application {
    public static Frame RootFrame;
    public static MainWindow MainWindow;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App() {
        InitializeComponent();

#if !DEBUG
        AppCenter.Start("3169a606-ee13-45f6-bb04-fc381ae6702f",
            typeof(Analytics), typeof(Crashes));
#endif
#if DEBUG
        AppCenter.Start("f6d4a673-0c33-4150-a751-cd1b7937b99d",
            typeof(Analytics), typeof(Crashes));
#endif

        Ioc.Default.ConfigureServices(ConfigureServices());
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args) {
        m_window = new MainWindow();
        m_window.Activate();
    }

    private Window m_window;

    private static IServiceProvider ConfigureServices() {
        ServiceCollection services = new();

        // ViewModels
        services
            // Controls
            .AddTransient<DataItemViewModel>()
            .AddTransient<MusicPlayerViewModel>()
            .AddTransient<UserFlyoutViewModel>()
            .AddTransient<TrackMusicDialogViewModel>()
            .AddTransient<MusicDataItemViewModel>()
            .AddTransient<CreatePlaylistDialogViewModel>()
            .AddTransient<ConfirmDeletePlaylistDialogViewModel>()
            .AddTransient<EditPlaylistInfoDialogViewModel>()
            .AddTransient<CommentViewModel>()
            .AddTransient<CommentItemViewModel>()
            .AddTransient<EditProfileDialogViewModel>()
            // FriendsPanel
            .AddTransient<FansViewModel>()
            .AddTransient<FollowersViewModel>()
            // Lyric
            .AddTransient<LyricViewViewModel>()
            // SearchPanel
            .AddTransient<MusicSearchPanelViewModel>()
            .AddTransient<PlaylistSearchPanelViewModel>()
            .AddTransient<ArtistSearchPanelViewModel>()
            .AddTransient<UserSearchPanelViewModel>()
            // NoticePanel
            .AddTransient<ReplyNoticePanelViewModel>()
            .AddTransient<SystemNoticePanelViewModel>()
            // Page
            .AddTransient<ArtistPageViewModel>()
            .AddTransient<DiscoverViewModel>()
            .AddTransient<FriendsViewModel>()
            .AddTransient<LibraryViewModel>()
            .AddTransient<LoginPageViewModel>()
            .AddTransient<MainPageViewModel>()
            .AddTransient<PlayingViewModel>()
            .AddTransient<PlaylistPageViewModel>()
            .AddTransient<ProfilePageViewModel>()
            .AddTransient<SearchViewModel>()
            .AddTransient<SettingsPageViewModel>()
            .AddTransient<NoticePageViewModel>()
            .AddTransient<CommentPageViewModel>();

        // Services
        services
            .AddSingleton<INavigationService, NavigatoinSerivce>()
            .AddSingleton<IMediaPlayBackService, MediaPlaybackService>()
            .AddSingleton<IAuthorizationService, AuthorizationService>(initAuthorizationService);

        services.AddRefitClient<IAppCenterReleasesService>(new RefitSettings {
            ContentSerializer = new NewtonsoftJsonContentSerializer()
        }).ConfigureHttpClient(options => {
            options.BaseAddress = new Uri("https://install.appcenter.ms");
        });

        services.AddRefitClient<IVtuberMusicService>(new RefitSettings {
            ContentSerializer = new NewtonsoftJsonContentSerializer(),
            AuthorizationHeaderValueGetter = auth
        })
            .ConfigureHttpClient(options => {
                options.BaseAddress = new Uri("https://api.aqua.chat");
            });

        return services.BuildServiceProvider();
    }

    private static AuthorizationService initAuthorizationService(IServiceProvider arg) {
        AuthorizationService service = new(SettingsHelper.RefreshToken);
        service.IsAuthorizedChanged += Service_IsAuthorizedChanged;
        service.IsLoginChanged += Service_IsLoginChanged;

        return service;
    }

    private static void Service_IsLoginChanged(object sender, bool e) {
        //throw new NotImplementedException();
    }

    private static void Service_IsAuthorizedChanged(object sender, bool e) {
        SettingsHelper.RefreshToken = (sender as IAuthorizationService).GetRefreshToken();
        AppCenter.SetUserId((sender as IAuthorizationService).Account.id);
    }

    private static async Task<string> auth() {
        var service = Ioc.Default.GetService<IAuthorizationService>();
        return await service.GetTokenAsync();
    }
}
