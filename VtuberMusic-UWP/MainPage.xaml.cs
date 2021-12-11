using System;
using System.Collections.Generic;
using System.Numerics;
using VtuberMusic_UWP.Components.Dialog;
using VtuberMusic_UWP.Models.Main;
using VtuberMusic_UWP.Pages;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VtuberMusic_UWP {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        public TypedEventHandler<object, NavigationEventArgs> Navigated;
        public Frame ContentFrame => this.NavigationFrame;
        private ViewModel ViewModel => App.ViewModel;

        private List<Microsoft.UI.Xaml.Controls.NavigationViewItem> MyAlbumNavigationItemList = new List<Microsoft.UI.Xaml.Controls.NavigationViewItem>();
        private List<Microsoft.UI.Xaml.Controls.NavigationViewItem> FavouriteNavigationItemList = new List<Microsoft.UI.Xaml.Controls.NavigationViewItem>();

        public MainPage() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            App.ViewModel.MainPage = this;

            this.Navigation.SelectionChanged += this.Navigation_SelectionChanged;
            this.NavigationFrame.Navigated += this.NavigationFrame_Navigated;
            this.Navigation.SelectedItem = this.Navigation.MenuItems[1];

            App.Player.NowPlayingMusicChanged += this.NowPlayingMusicChanged;
            this.PlayerOut.Completed += delegate {
                if (App.Player.NowPlayingMusic == null)
                    this.MainPlayer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                else
                    this.PlayerIn.Begin();
            };

            this.Load();
        }

        private async void NowPlayingMusicChanged(object sender, Models.VtuberMusic.Music e) {
            await this.Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate {
                if (e == null) {
                    this.PlayerOut.Begin();
                } else {
                    if (this.MainPlayer.Visibility == Windows.UI.Xaml.Visibility.Visible) return;

                    this.MainPlayer.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    this.PlayerIn.Begin();
                }
            }));
        }

        #region 加载导航歌单列表
        /// <summary>
        /// 加载导航侧边栏歌单列表
        /// </summary>
        public async void Load() {
            var likeMusicAlbum = await App.Client.Account.GetLikeMusicSong();
            this.LikeMusic.Tag = new NavigationItemTag {
                PageType = typeof(Album),
                Args = new AlbumPageArgs { Album = likeMusicAlbum.Data.playlist, IsLikeMusic = true }
            };

            var myAlbum = await App.Client.Account.GetMyCreatePlayList();
            var myFavouriteAlbum = await App.Client.Account.GetMyFavouritePlayList();

            foreach (var item in this.MyAlbumNavigationItemList) {
                this.Navigation.MenuItems.Remove(item);
            }

            this.MyAlbumNavigationItemList.Clear();

            foreach (var item in this.FavouriteNavigationItemList) {
                this.Navigation.MenuItems.Remove(item);
            }

            this.FavouriteNavigationItemList.Clear();

            var myAlbumHeaderIndex = this.Navigation.MenuItems.IndexOf(this.MyAlbumTitle);
            foreach (var album in myAlbum.Data) {
                myAlbumHeaderIndex++;
                if (album.privacy == 0) {
                    var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem {
                        Icon = new SymbolIcon(Symbol.MusicInfo),
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    };

                    this.Navigation.MenuItems.Insert(myAlbumHeaderIndex, item);
                    this.MyAlbumNavigationItemList.Add(item);
                } else {
                    var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem {
                        Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\uE1F6" },
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    };

                    this.Navigation.MenuItems.Insert(myAlbumHeaderIndex, item);
                    this.MyAlbumNavigationItemList.Add(item);
                }
            }

            var myFavouriteAlbumIndex = this.Navigation.MenuItems.IndexOf(this.FavouriteAlbum) + 1;
            for (int i = myFavouriteAlbum.Data.Length - 1; i != -1; i--) {
                var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem {
                    Icon = new SymbolIcon(Symbol.MusicInfo),
                    Content = myFavouriteAlbum.Data[i].name,
                    Tag = new NavigationItemTag { PageType = typeof(Album), Args = myFavouriteAlbum.Data[i] }
                };

                this.Navigation.MenuItems.Insert(myFavouriteAlbumIndex, item);
                this.FavouriteNavigationItemList.Add(item);

                myFavouriteAlbumIndex++;
            }
        }
        #endregion

        #region 导航
        private void Navigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
            if (args.IsSettingsSelected) {
                this.ContentFrame.Navigate(typeof(Settings));
            }

            var item = (Microsoft.UI.Xaml.Controls.NavigationViewItemBase)args.SelectedItem;
            if (item != null && item.Tag != null && item.Tag is NavigationItemTag) {
                var tag = (NavigationItemTag)item.Tag;
                var page = (Page)this.ContentFrame.Content;

                if (page == null || tag.PageType != page.GetType() || tag.Args != page.Tag) this.ContentFrame.Navigate(tag.PageType, tag.Args);
            }
        }

        private void NavigationFrame_Navigated(object sender, NavigationEventArgs e) {
            this.Navigation.IsBackEnabled = this.ContentFrame.CanGoBack;

            if (e.Content is Settings) {
                this.Navigation.SelectedItem = this.Navigation.SettingsItem;
                return;
            }

            foreach (Microsoft.UI.Xaml.Controls.NavigationViewItemBase tmp in this.Navigation.MenuItems) {
                var tag = (NavigationItemTag)tmp.Tag;
                if (tmp.Tag != null && tag.PageType == e.Content.GetType() && tag.Args == e.Parameter) {
                    this.Navigation.SelectedItem = tmp;
                    return;
                }
            }

            this.Navigation.SelectedItem = null;
        }
        #endregion

        private void MainPlayer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            this.ShareShadow.Receivers.Add(this.NavigationFrame);
            this.MainPlayer.Translation = new Vector3(0, 0, 25);
        }

        private void Navigation_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args) =>
            this.NavigationFrame.GoBack();

        private async void AddAlbum_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => await App.ContentDialogManager.ShowAsync(new CreateAlbumDialog());
    }

    [MarkupExtensionReturnType(ReturnType = typeof(NavigationItemTag))]
    public class NavigationItemTag : MarkupExtension {
        protected override object ProvideValue() => this;
        public Type PageType { get; set; }
        public object Args { get; set; }
    }
}
