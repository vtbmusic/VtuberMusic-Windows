using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Refit;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Pages;
using VtuberMusic.App.Services;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic.App {
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application {
        public IServiceProvider ServiceProvider;
        public Frame RootFrame;

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            Ioc.Default.ConfigureServices(ConfigureServices());
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            TitleBarHelper.CoreTitleBar.ExtendViewIntoTitleBar = true;
            TitleBarHelper.ChangeTheme(this.RequestedTheme);
            DispatcherHelper.Init();

            if (Window.Current.Content is null) {
                RootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                Window.Current.Content = RootFrame;
                if (Ioc.Default.GetService<IAuthorizationService>().IsLogin) {
                    RootFrame.Navigate(typeof(MainPage));
                } else {
                    RootFrame.Navigate(typeof(LoginPage));
                }
            }

            if (e.PrelaunchActivated == false) {
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        private static IServiceProvider ConfigureServices() {
            var services = new ServiceCollection();

            services.AddSingleton<INavigationService, NavigatoinSerivce>();
            services.AddSingleton<IMediaPlayBackService, MediaPlaybackService>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>(initAuthorizationService);
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
            var service = new AuthorizationService(SettingsHelper.RefreshToken);
            service.IsAuthorizedChanged += Service_IsAuthorizedChanged;
            service.IsLoginChanged += Service_IsLoginChanged;

            return service;
        }

        private static void Service_IsLoginChanged(object sender, bool e) {
            //throw new NotImplementedException();
        }

        private static void Service_IsAuthorizedChanged(object sender, bool e) {
            SettingsHelper.RefreshToken = (sender as IAuthorizationService).GetRefreshToken();
        }

        private async static Task<string> auth() {
            var service = Ioc.Default.GetService<IAuthorizationService>();
            return await service.GetTokenAsync();
        }
    }
}