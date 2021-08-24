using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using VtuberMusic_UWP.Components.Dialog;
using VtuberMusic_UWP.Pages;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VtuberMusic_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public TypedEventHandler<object, NavigationEventArgs> Navigated;
        public Frame ContentFrame => NavigationFrame;

        private List<Microsoft.UI.Xaml.Controls.NavigationViewItem> MyAlbumNavigationItemList = new List<Microsoft.UI.Xaml.Controls.NavigationViewItem>();
        private List<Microsoft.UI.Xaml.Controls.NavigationViewItem> FavouriteNavigationItemList = new List<Microsoft.UI.Xaml.Controls.NavigationViewItem>();

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            App.ViewModel.MainPage = this;

            Navigation.SelectionChanged += Navigation_SelectionChanged;
            NavigationFrame.Navigated += NavigationFrame_Navigated;
            Navigation.SelectedItem = Navigation.MenuItems[1];

            App.Player.NowPlayingMusicChanged += NowPlayingMusicChanged;
            PlayerOut.Completed += delegate
            {
                MainPlayer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            };

            Load();
        }

        private async void NowPlayingMusicChanged(object sender, Models.VtuberMusic.Music e)
        {
            await Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate
            {
                if (e == null)
                {
                    PlayerOut.Begin();
                }
                else
                {
                    if (MainPlayer.Visibility == Windows.UI.Xaml.Visibility.Visible) return;

                    MainPlayer.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    PlayerIn.Begin();
                }
            }));
        }

        #region 加载导航歌单列表
        public async void Load()
        {
            var likeMusicAlbum = await App.Client.Account.GetLikeMusicSong();
            LikeMusic.Tag = new NavigationItemTag
            {
                PageType = typeof(Album),
                Args = new AlbumPageArgs { Album = likeMusicAlbum.Data.playlist, IsLikeMusic = true }
            };

            var myAlbum = await App.Client.Account.GetMyCreatePlayList();
            var myFavouriteAlbum = await App.Client.Account.GetMyFavouritePlayList();

            foreach (var item in MyAlbumNavigationItemList)
            {
                Navigation.MenuItems.Remove(item);
            }
            MyAlbumNavigationItemList.Clear();

            foreach (var item in FavouriteNavigationItemList)
            {
                Navigation.MenuItems.Remove(item);
            }
            FavouriteNavigationItemList.Clear();

            var myAlbumHeaderIndex = Navigation.MenuItems.IndexOf(MyAlbumTitle);
            foreach (var album in myAlbum.Data)
            {
                myAlbumHeaderIndex++;
                if (album.privacy == 0)
                {
                    var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem
                    {
                        Icon = new SymbolIcon(Symbol.MusicInfo),
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    };

                    Navigation.MenuItems.Insert(myAlbumHeaderIndex, item);
                    MyAlbumNavigationItemList.Add(item);
                }
                else
                {
                    var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem
                    {
                        Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\uE1F6" },
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    };

                    Navigation.MenuItems.Insert(myAlbumHeaderIndex, item);
                    MyAlbumNavigationItemList.Add(item);
                }
            }

            var myFavouriteAlbumIndex = Navigation.MenuItems.IndexOf(FavouriteAlbum) + 1;
            for (int i = myFavouriteAlbum.Data.Length - 1; i != -1; i--)
            {
                var item = new Microsoft.UI.Xaml.Controls.NavigationViewItem
                {
                    Icon = new SymbolIcon(Symbol.MusicInfo),
                    Content = myFavouriteAlbum.Data[i].name,
                    Tag = new NavigationItemTag { PageType = typeof(Album), Args = myFavouriteAlbum.Data[i] }
                };

                Navigation.MenuItems.Insert(myFavouriteAlbumIndex, item);
                FavouriteNavigationItemList.Add(item);

                myFavouriteAlbumIndex++;
            }
        }
        #endregion

        #region 导航
        private void Navigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(Settings));
            }

            var item = (Microsoft.UI.Xaml.Controls.NavigationViewItemBase)args.SelectedItem;
            if (item != null && item.Tag != null && item.Tag is NavigationItemTag)
            {
                var tag = (NavigationItemTag)item.Tag;
                var page = (Page)ContentFrame.Content;

                if (page == null || tag.PageType != page.GetType() || tag.Args != page.Tag) ContentFrame.Navigate(tag.PageType, tag.Args);
            }
        }

        private void NavigationFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigation.IsBackEnabled = ContentFrame.CanGoBack;

            if (e.Content is Settings)
            {
                Navigation.SelectedItem = Navigation.SettingsItem;
                return;
            }

            foreach (Microsoft.UI.Xaml.Controls.NavigationViewItemBase tmp in Navigation.MenuItems)
            {
                var tag = (NavigationItemTag)tmp.Tag;
                if (tmp.Tag != null && tag.PageType == e.Content.GetType() && tag.Args == e.Parameter)
                {
                    Navigation.SelectedItem = tmp;
                    return;
                }
            }

            Navigation.SelectedItem = null;
        }
        #endregion

        private void MainPlayer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ShareShadow.Receivers.Add(NavigationFrame);
            MainPlayer.Translation = new Vector3(0, 0, 25);
        }

        private void Navigation_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args) =>
            NavigationFrame.GoBack();

        private void AddAlbum_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => new CreateAlbumDialog().ShowAsync();
    }

    [MarkupExtensionReturnType(ReturnType = typeof(NavigationItemTag))]
    public class NavigationItemTag : MarkupExtension
    {
        protected override object ProvideValue() => this;
        public Type PageType { get; set; }
        public object Args { get; set; }
    }
}
