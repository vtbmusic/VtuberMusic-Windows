using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic.App.PageArgs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Search : Page {
        public Search() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            ViewModel.Keyword = (e.Parameter as SearchPageArg).Keyword;
        }

        private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
            hideSearch();
            if (Nav.SelectedItem == MusicItem) {
                ViewModel.IsSearchMusic = true;
            } else if (Nav.SelectedItem == ArtistItem) {
                ViewModel.IsSearchArtist = true;
            } else if (Nav.SelectedItem == PlaylistItem) {
                ViewModel.IsSearchPlaylist = true;
            } else if (Nav.SelectedItem == UserItem) {
                ViewModel.IsSearchUser = true;
            }
        }

        private void hideSearch() {
            ViewModel.IsSearchArtist = false;
            ViewModel.IsSearchMusic = false;
            ViewModel.IsSearchPlaylist = false;
            ViewModel.IsSearchUser = false;
        }
    }
}
