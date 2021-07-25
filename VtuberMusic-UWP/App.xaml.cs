using System;
using VtuberMusic_UWP.Models.Main;
using VtuberMusic_UWP.Pages;
using VtuberMusic_UWP.Service;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP
{
    sealed partial class App : Application
    {
        public static ViewModel ViewModel = new ViewModel();
        public static MusicClient Client = new MusicClient();
        public static Player Player = new Player();
        public static Frame RootFrame;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // 扩展到标题栏
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            RootFrame = Window.Current.Content as Frame;

            if (RootFrame == null)
            {
                RootFrame = new Frame();

                RootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = RootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                Window.Current.Activate();
            }

            RootFrame.Navigate(typeof(Setup));
            //rootFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
    }
}
