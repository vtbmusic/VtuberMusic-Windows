using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VtuberMusic_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool playingOpen = false;

        public MainPage()
        {
            this.InitializeComponent();
            App.ViewModel.MainPage = this;
            App.Player.NowPlayingMusicChanged += nowPlayingMusicChanged;
        }

        private void update()
        {
            if (App.Player.NowPlayingMusic != null)
            {
                var image = new BitmapImage();
                image.UriSource = new Uri(App.Player.NowPlayingMusic.ResourcesUrl.CoverImg);
                Background = new ImageBrush { ImageSource = image, Stretch = Stretch.UniformToFill };
            }
            else
            {
                Background = null;
            }
        }

        private async void nowPlayingMusicChanged(object sender, MusicData e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(update));
        }
    }
}
