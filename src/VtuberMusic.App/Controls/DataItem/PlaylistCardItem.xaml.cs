using Microsoft.Toolkit.Mvvm.DependencyInjection;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.Services;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic.App.Controls.DataItem {
    public sealed partial class PlaylistCardItem : UserControl {
        public static readonly DependencyProperty PlaylistProperty =
            DependencyProperty.Register("Playlist", typeof(Playlist), typeof(PlaylistCardItem), new PropertyMetadata(null));

        public static readonly DependencyProperty PlaylistTypeProperty =
            DependencyProperty.Register("PlaylistType", typeof(PlaylistType), typeof(PlaylistCardItem), new PropertyMetadata(PlaylistType.Playlist));

        public Playlist Playlist {
            get { return GetValue(PlaylistProperty) as Playlist; }
            set { SetValue(PlaylistProperty, value); }
        }

        public PlaylistType PlaylistType {
            get { return (PlaylistType)GetValue(PlaylistTypeProperty); }
            set { SetValue(PlaylistTypeProperty, value); }
        }

        private INavigationService _navigationService = Ioc.Default.GetService<INavigationService>();

        public PlaylistCardItem() {
            this.InitializeComponent();
        }

        private void NavigateToPlaylist_Click(object sender, RoutedEventArgs e) {
            _navigationService.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = Playlist, PlaylistType = PlaylistType });
        }

        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            VisualStateManager.GoToState(this, "Hover", true);
        }

        private void Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {
            VisualStateManager.GoToState(this, "Normal", true);
        }
    }
}
