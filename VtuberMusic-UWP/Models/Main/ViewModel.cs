using System;
using VtuberMusic_UWP.Components.Main;
using VtuberMusic_UWP.Components.Player;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace VtuberMusic_UWP.Models.Main {
    /// <summary>
    /// ViewModel
    /// </summary>
    public class ViewModel {
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
        public Uri BackgroundImageUri { private set; get; }

        /// <summary>
        /// 设置 App 背景图片
        /// </summary>
        /// <param name="imageUri">图片 Uri</param>
        public async void SetAppBackgroundImage(Uri imageUri) {
            this.BackgroundImageUri = imageUri;

            await this.MainPage.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate {
                var image = new BitmapImage();
                this.MainPage.Background = new ImageBrush { ImageSource = image, Stretch = Stretch.UniformToFill };

                image.DecodePixelHeight = (int)Window.Current.Bounds.Height / 4;
                image.DecodePixelWidth = (int)Window.Current.Bounds.Width / 4;
                image.UriSource = imageUri;
            });
        }

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
    }
}
