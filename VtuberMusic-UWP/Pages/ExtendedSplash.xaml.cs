using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 启动加载动画页
    /// </summary>
    public sealed partial class ExtendedSplash : Page {
        internal Rect splashImageRect;
        private SplashScreen splash;
        internal bool dismissed = false;
        internal Frame rootFrame;

        public ExtendedSplash(SplashScreen splashScreen, bool loadState) {
            this.InitializeComponent();

            this.splash = splashScreen;
            if (this.splash != null) {
                this.splash.Dismissed += this.Splash_Dismissed;

                this.splashImageRect = this.splash.ImageLocation;

            }
        }

        private void Splash_Dismissed(SplashScreen sender, object args) {
            this.dismissed = true;
        }

        async void DismissExtendedSplash() {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                this.rootFrame = new Frame();
                this.rootFrame.Content = new MainPage(); Window.Current.Content = this.rootFrame;
            });
        }
    }
}
