using System;
using VtuberMusic_UWP.Components.Main;
using VtuberMusic_UWP.Components.Player;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VtuberMusic_UWP.Models.Main
{
    public class ViewModel
    {
        public MainPage MainPage;
        public SidePanel SidePanel;
        public MainPlayer MainPlayer;

        public Album CurrentAlbum;

        public void NavigateToPage(Type page, object args = null)
        {
            SidePanel.NavigateToPage(page, args);
        }

        public async void SetAppBackgroundImage(Uri imageUri)
        {
            await MainPage.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
            {
                MainPage.Background = new ImageBrush { ImageSource = new BitmapImage(imageUri) };
            });
        }
    }
}
