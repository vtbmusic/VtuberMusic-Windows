using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using VtuberMusic_UWP.Components;
using VtuberMusic_UWP.Components.Dialog;
using VtuberMusic_UWP.Models.DebugCommand;
using VtuberMusic_UWP.Models.Main;
using VtuberMusic_UWP.Pages;
using VtuberMusic_UWP.Service;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP {
    sealed partial class App : Application {
        public static ViewModel ViewModel = new ViewModel();
        public static MusicClient Client = new MusicClient();
        public static Player Player = new Player();
        public static Frame RootFrame;
        public static RestClient PublicClient = new RestClient();
        public static DebugCommandManager DebugCommandManager = new DebugCommandManager();

        public App() {
            this.InitializeComponent();

            App.Current.UnhandledException += this.Current_UnhandledException;
            Client.Account.LoginStatueChanged += this.Account_LoginStatueChanged;
        }

        private void Account_LoginStatueChanged(object sender, Models.VtuberMusic.AccountProfileData e) {
            if (e.account != null) AppCenter.SetUserId(e.account.id);
        }

        private async void Current_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e) {
            e.Handled = true;

            var data = new Dictionary<string, string>()
            {
                { "Crash_Message", e.Message }
            };

            Crashes.TrackError(e.Exception, data);

            await Window.Current.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                if (e.Exception.StackTrace == null) {
                    InfoBarPopup.Show("发生了一个异常", e.Exception.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                } else {
                    InfoBarPopup.Show($"发生了一个异常: { e.Exception.Message }", e.Exception.StackTrace, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                }
            }));
        }

        private async void showCrashReport() {
            await RootFrame.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(async delegate {
                var dialog = new ContentDialog();
                dialog.Title = "是否要上传错误报告？";
                dialog.PrimaryButtonText = "总是";
                dialog.SecondaryButtonText = "上传";
                dialog.CloseButtonText = "不上传";
                dialog.Content = new TextBlock() { Text = "上传错误报告以帮助我们修复此问题" };
                dialog.DefaultButton = ContentDialogButton.Primary;

                var result = await dialog.ShowAsync();

                switch (result) {
                    case ContentDialogResult.Primary:
                        Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);
                        break;
                    case ContentDialogResult.Secondary:
                        Crashes.NotifyUserConfirmation(UserConfirmation.Send);
                        break;
                    case ContentDialogResult.None:
                        Crashes.NotifyUserConfirmation(UserConfirmation.DontSend);
                        break;
                }
            }));
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;

            // 扩展到标题栏
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            RootFrame = Window.Current.Content as Frame;

            if (RootFrame == null) {
                RootFrame = new Frame();

                RootFrame.NavigationFailed += this.OnNavigationFailed;

                Window.Current.Content = RootFrame;
            }

            if (e.PrelaunchActivated == false) {
                Window.Current.Activate();
            }

            if (e.PreviousExecutionState != ApplicationExecutionState.Running) {
                bool loadState = ( e.PreviousExecutionState == ApplicationExecutionState.Terminated );
                ExtendedSplash extendedSplash = new ExtendedSplash(e.SplashScreen, loadState);
                RootFrame.Content = extendedSplash;
            }

            this.init();
            this.initAppCenter();
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args) {
            if (Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftControl) != CoreVirtualKeyStates.None &&
                Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftShift) != CoreVirtualKeyStates.None &&
                args.VirtualKey == VirtualKey.P) {
                new DebugPanel().Show();
            }
        }

        private async void init() {
            var request = new RestRequest("https://vtbmusic.github.io/UWP_UpdateCheck/update.json");
            var response = await PublicClient.ExecuteAsync<UpdateCheck>(request);

            if (response.IsSuccessful &&
                (response.Data.version != Assembly.GetExecutingAssembly().GetName().Version.ToString() || response.Data.commit != this.getGitCommitInfo() )) {
                await new UpdateCheckDialog(response.Data).ShowAsync();
#if !DEBUG
                Environment.Exit(0);
#endif
            }

            var username = (string)ApplicationData.Current.LocalSettings.Values["Username"];
            var password = (string)ApplicationData.Current.LocalSettings.Values["Password"];
            if (username != null && password != null) {
                try {
                    await Client.Account.Login(username, password);
                    RootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
                } catch {
                    RootFrame.Navigate(typeof(Setup), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                }
            } else {
                RootFrame.Navigate(typeof(Setup), null, new DrillInNavigationTransitionInfo());
            }
        }

        private void initAppCenter() {
            var countryCode = new GeographicRegion().CodeTwoLetter;
            AppCenter.SetCountryCode(countryCode);
#if DEBUG
            AppCenter.Start("45808951-480e-4cf7-9fb3-e7c325c68836",
                       typeof(Analytics), typeof(Crashes));

            var properties = new CustomProperties();
            properties.Set("Commit", this.getGitCommitInfo());

            AppCenter.SetCustomProperties(properties);

            Crashes.ShouldAwaitUserConfirmation = () => {
                this.showCrashReport();
                return true;
            };
#endif
#if !DEBUG
            AppCenter.Start("b70c28c4-5a3a-4416-8eac-72106776951d",
                       typeof(Analytics), typeof(Crashes));

            var properties = new CustomProperties();
            properties.Set("Commit", this.getGitCommitInfo());

            AppCenter.SetCustomProperties(properties);

            Crashes.ShouldAwaitUserConfirmation = () =>
            {
                this.showCrashReport();
                return true;
            };
#endif
        }

        private string getGitCommitInfo() {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
            return attributes.Length != 0 ? ( (AssemblyInformationalVersionAttribute)attributes[0] ).InformationalVersion : "Nan";
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            e.Handled = true;

            var data = new Dictionary<string, string>()
            {
                { "page", e.SourcePageType.FullName }
            };

            Crashes.TrackError(e.Exception, data);
        }
    }
}
