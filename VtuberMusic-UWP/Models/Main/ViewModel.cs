using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VtuberMusic_UWP.Components.Main;
using VtuberMusic_UWP.Components.Player;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace VtuberMusic_UWP.Models.Main {
    /// <summary>
    /// ViewModel
    /// </summary>
    public class ViewModel : INotifyPropertyChanged {
        /// <summary>
        /// App 主页面
        /// </summary>
        public MainPage MainPage;
        /// <summary>
        /// App 顶部面板
        /// </summary>
        public TopPanel TopPanel;
        /// <summary>
        /// App 主播放器控件
        /// </summary>
        public MainPlayer MainPlayer;
        /// <summary>
        /// App 背景 Uri
        /// </summary>
        public string BackgroundImageUri {
            get { return backgroundImageUri; }
            set {
                backgroundImageUri = value;
                this.NotifyPropertyChanged();
            }
        }

        private string backgroundImageUri = "/Assets/SetupBackground/66681407_p0.jpg";

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// App 主页面 Frame 导航到页面
        /// </summary>
        /// <param name="page">页面类型</param>
        /// <param name="args">参数</param>
        /// <param name="navigationTransitionInfo">导航动画</param>
        public void NavigateToPage(Type page, object args = null, NavigationTransitionInfo navigationTransitionInfo = null) {
            var transition = navigationTransitionInfo;
            if (navigationTransitionInfo == null) transition = new DrillInNavigationTransitionInfo();
            this.MainPage.ContentFrame.Navigate(page, args, transition);
        }

        private async void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            await App.RootFrame.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
    }
}
