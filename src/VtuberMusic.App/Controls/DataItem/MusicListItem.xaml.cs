using VtuberMusic.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem {
    public sealed partial class MusicListItem : UserControl {
        public static readonly DependencyProperty MusicProperty =
            DependencyProperty.Register("Music", typeof(Music), typeof(MusicListItem), new PropertyMetadata(null, new PropertyChangedCallback(MusicPropertyChanged)));

        private static void MusicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var musicItem = d as MusicListItem;
            musicItem.ArtistMenuFlyoutItem.Items.Clear();

            foreach (var item in musicItem.Music.artists) {
                musicItem.ArtistMenuFlyoutItem.Items.Add(new MenuFlyoutItem { Text = item.name.origin, Command = musicItem.ViewModel.NavigateToArtistCommand, CommandParameter = item });
            }
        }

        public Music Music {
            get { return GetValue(MusicProperty) as Music; }
            set { SetValue(MusicProperty, value); }
        }

        public MusicListItem() {
            this.InitializeComponent();
        }
    }
}
