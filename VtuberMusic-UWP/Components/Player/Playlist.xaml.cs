using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Player {
    /// <summary>
    /// 播放列表控件
    /// </summary>
    public sealed partial class Playlist : UserControl {
        public Playlist() {
            this.InitializeComponent();

            App.Player.NowPlayingMusicChanged += this.NowPlayingMusicChanged;
            App.Player.PlayList.CollectionChanged += this.PlayList_CollectionChanged;
            this.DataView.ItemsSource = App.Player.PlayList;

            if (App.Player.PlayList.Count == 0) this.None.Visibility = Visibility.Visible;
        }

        private void PlayList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            this.None.Visibility = App.Player.PlayList.Count != 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void NowPlayingMusicChanged(object sender, Music e) {
            if (App.Player.PlayList.Count == 0) this.None.Visibility = Visibility.Visible;
        }

        private void Remove_Click(object sender, RoutedEventArgs e) => App.Player.PlayList.Remove((Music)this.DataView.SelectedItem);
        private void DeleteMusic_Click(object sender, RoutedEventArgs e) => App.Player.PlayList.Remove((Music)( (AppBarButton)sender ).Tag);
        private void DeleteSwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) => App.Player.PlayList.Remove((Music)args.SwipeControl.Tag);
        private void DataView_Loaded(object sender, RoutedEventArgs e) => this.DataView.ScrollIntoView(App.Player.NowPlayingMusic, ScrollIntoViewAlignment.Leading);
        private void ArtistButton_Click(object sender, RoutedEventArgs e) => App.ViewModel.NavigateToPage(typeof(Pages.Artist), ( (HyperlinkButton)sender ).Tag);
    }
}
