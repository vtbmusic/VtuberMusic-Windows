using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
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
        public Frame PageFrame => NavigationFrame;
        public Frame ContentFrame => NavigationFrame;

        public MainPage()
        {
            this.InitializeComponent();
            App.ViewModel.MainPage = this;
            App.ViewModel.TopPanel.Init();

            Navigation.SelectionChanged += Navigation_SelectionChanged;
            NavigationFrame.Navigated += NavigationFrame_Navigated;
            Navigation.ItemInvoked += Navigation_ItemInvoked;

            Navigation.SelectedItem = Navigation.MenuItems[1];

            Load();
        }

        #region 加载导航歌单列表
        private async void Load()
        {
            var myAlbum = await App.Client.Account.GetMyCreatePlayList();
            var myFavouriteAlbum = await App.Client.Account.GetMyFavouritePlayList();

            var myAlbumHeaderIndex = Navigation.MenuItems.IndexOf(MyAlbumTitle) + 1;
            foreach (var album in myAlbum.Data)
            {
                if (album.privacy == 0)
                {
                    Navigation.MenuItems.Insert(myAlbumHeaderIndex, new NavigationViewItem
                    {
                        Icon = new SymbolIcon(Symbol.MusicInfo),
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    });
                }
                else
                {
                    Navigation.MenuItems.Insert(myAlbumHeaderIndex, new NavigationViewItem
                    {
                        Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\uE1F6" },
                        Content = album.name,
                        Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                    });
                }
            }

            var myFavouriteAlbumIndex = Navigation.MenuItems.IndexOf(FavouriteAlbum) + 1;
            foreach (var album in myFavouriteAlbum.Data)
            {
                Navigation.MenuItems.Insert(myFavouriteAlbumIndex, new NavigationViewItem
                {
                    Icon = new SymbolIcon(Symbol.MusicInfo),
                    Content = album.name,
                    Tag = new NavigationItemTag { PageType = typeof(Album), Args = album }
                });
            }
        }
        #endregion

        #region 导航
        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = (NavigationViewItemBase)args.SelectedItem;
            if (item != null && item.Tag != null && item.Tag is NavigationItemTag)
            {
                var tag = (NavigationItemTag)item.Tag;
                var page = (Page)ContentFrame.Content;

                if (page == null) ContentFrame.Navigate(tag.PageType, tag.Args);
                if (page != null && tag.Args != page.Tag) ContentFrame.Navigate(tag.PageType, tag.Args);
            }
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked && ContentFrame.Content.GetType() != typeof(Settings))
            {
                ContentFrame.Navigate(typeof(Settings));
                return;
            }

            var item = args.InvokedItemContainer;
            if (item.Tag is NavigationItemTag)
            {
                var tag = (NavigationItemTag)item.Tag;
                var page = (Page)ContentFrame.Content;

                if (page == null) ContentFrame.Navigate(tag.PageType, tag.Args);
                if (page != null && tag.Args != page.Tag) ContentFrame.Navigate(tag.PageType, tag.Args);
            }
        }

        private void NavigationFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Settings)
            {
                Navigation.SelectedItem = Navigation.SettingsItem;
                return;
            }

            foreach (NavigationViewItemBase tmp in Navigation.MenuItems)
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

        #region 插入导航菜单歌单
        public void InsertMyCreateAlbum(Models.VtuberMusic.Album album)
        {

        }

        public void InsertFavouriteAlbum(Models.VtuberMusic.Album album)
        {

        }
        #endregion

        private void MainPlayer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ShareShadow.Receivers.Add(NavigationFrame);
            MainPlayer.Translation = new Vector3(0, 0, 25);
        }
    }

    [MarkupExtensionReturnType(ReturnType = typeof(NavigationItemTag))]
    public class NavigationItemTag : MarkupExtension
    {
        protected override object ProvideValue() => this;
        public Type PageType { get; set; }
        public object Args { get; set; }
    }
}
