using System;
using VtuberMusic_UWP.Components.Main;
using VtuberMusic_UWP.Components.Player;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace VtuberMusic_UWP.Models.Main
{
    public class ViewModel
    {
        public MainPage MainPage;
        public TopPanel TopPanel;
        public MainPlayer MainPlayer;
        public Uri BackgroundImageUri { private set; get; }

        public async void SetAppBackgroundImage(Uri imageUri)
        {
            BackgroundImageUri = imageUri;

            await MainPage.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
            {
                var image = new BitmapImage();
                MainPage.Background = new ImageBrush { ImageSource = image, Stretch = Stretch.UniformToFill };

                image.DecodePixelHeight = (int)Window.Current.Bounds.Height / 4;
                image.DecodePixelWidth = (int)Window.Current.Bounds.Width / 4;
                image.UriSource = imageUri;
            });
        }

        public void NavigateToPage(Type page, object args = null, NavigationTransitionInfo navigationTransitionInfo = null)
        {
            var transition = navigationTransitionInfo;
            if (navigationTransitionInfo == null) transition = new DrillInNavigationTransitionInfo();
            MainPage.ContentFrame.Navigate(page, args, transition);
        }
    }
}
