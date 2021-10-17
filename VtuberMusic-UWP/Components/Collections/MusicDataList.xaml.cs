using Microsoft.AppCenter.Crashes;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace VtuberMusic_UWP.Components.Collections {
    /// <summary>
    /// 音乐列表
    /// </summary>
    public partial class MusicDataList : UserControl {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<Music>), typeof(MusicDataList), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable<Music> ItemsSource {
            get { return (IEnumerable<Music>)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 歌单列表
        /// </summary>
        public Album[] albums = null;
        private List<MenuFlyoutItem> flyoutItems = new List<MenuFlyoutItem>();

        public MusicDataList() {
            this.InitializeComponent();
        }

        private async void load() => this.albums = ( await App.Client.Account.GetMyCreatePlayList() ).Data;
        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e) => ( (MusicDataList)d ).ItemsSourceChange(e);

        private protected void ItemsSourceChange(DependencyPropertyChangedEventArgs e) {
            this.DataList.ItemsSource = e.NewValue;
        }

        private void DataList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
            if (this.DataList.SelectedItem != null) {
                App.Player.SetMusic((Music)this.DataList.SelectedItem);
            }
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e) {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
                VisualStateManager.GoToState(sender as Control, "Hover", true);
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e) =>
            VisualStateManager.GoToState(sender as Control, "Normal", true);

        private void ArtistButton_Click(object sender, RoutedEventArgs e) => App.ViewModel.NavigateToPage(typeof(Pages.Artist), ( (HyperlinkButton)sender ).Tag);
        private void Play_Click(object sender, RoutedEventArgs e) => App.Player.SetMusic((Music)( sender as Control ).Tag);

        private async void Like_Click(object sender, RoutedEventArgs e) {
            var button = (Button)sender;
            var music = (Music)button.Tag;
            button.IsEnabled = false;

            try {
                await App.Client.Account.LikeMusic(music.id, !music.like);
                ( (Music)button.Tag ).like = !music.like;

                button.IsEnabled = true;
            } catch (Exception ex) {
                button.IsEnabled = true;
                InfoBarPopup.Show("无法喜欢音乐", ex.Message);

                var data = new Dictionary<string, string>()
                {
                    { "Music_Id",music.id },
                    { "Like", (!music.like).ToString() }
                };

                Crashes.TrackError(ex, data);
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e) {
            var button = (Button)sender;
            var music = (Music)button.Tag;
            var menuFlyout = new MenuFlyout() { Placement = FlyoutPlacementMode.Bottom };
            var nextItem = new MenuFlyoutItem {
                Icon = new SymbolIcon(Symbol.Next),
                Text = "下一首播放",
                Tag = new FlyoutItemTag { Mode = FlyoutItemMode.AddNextPlay, Music = music },
            };

            nextItem.Click += this.FlyoutItem_Click;
            menuFlyout.Items.Add(nextItem);
            this.flyoutItems.Add(nextItem);
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            foreach (var item in this.albums) {
                var flyoutItem = new MenuFlyoutItem {
                    Text = item.name,
                    Icon = new SymbolIcon(Symbol.MusicInfo),
                    Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Add, AlbumId = item.id, Music = music }
                };

                flyoutItem.Click += this.FlyoutItem_Click;
                menuFlyout.Items.Add(flyoutItem);
                this.flyoutItems.Add(flyoutItem);
            }

            button.Flyout = menuFlyout;
        }

        private void Share_Click(object sender, RoutedEventArgs e) => ShareTools.ShareMusic((Music)( (Control)sender ).Tag);

        private async void FlyoutItem_Click(object sender, RoutedEventArgs e) {
            var tag = (FlyoutItemTag)( (MenuFlyoutItem)sender ).Tag;
            switch (tag.Mode) {
                case FlyoutItemMode.AddNextPlay:
                    if (!App.Player.PlayList.Contains(tag.Music)) {
                        int index = 0;
                        if (App.Player.PlayList.Contains(App.Player.NowPlayingMusic)) index = App.Player.PlayList.IndexOf(App.Player.NowPlayingMusic) + 1;
                        App.Player.PlayList.Insert(index, tag.Music); ;

                        if (App.Player.NowPlayingMusic == null) App.Player.SetMusic(App.Player.PlayList[0]);
                    } else {
                        InfoBarPopup.Show("添加失败", "歌曲已存在", InfoBarSeverity.Error);
                    }

                    break;
                case FlyoutItemMode.Add:
                    try {
                        await App.Client.Account.TrackMusic(tag.AlbumId, Service.TrackType.add, new string[] { tag.Music.id });
                        InfoBarPopup.Show("添加成功", "", InfoBarSeverity.Success);
                    } catch (Exception ex) {
                        InfoBarPopup.Show("无法添加到歌单", ex.Message, InfoBarSeverity.Error);
                    }

                    break;
                case FlyoutItemMode.Play:
                    App.Player.SetMusic(tag.Music);
                    break;
                case FlyoutItemMode.Like:
                    try {
                        await App.Client.Account.LikeMusic(tag.Music.id);
                        InfoBarPopup.Show("喜欢成功", "", InfoBarSeverity.Success);
                    } catch (Exception ex) {
                        InfoBarPopup.Show("喜欢音乐失败", ex.Message, InfoBarSeverity.Error);
                    }

                    break;
                case FlyoutItemMode.Share:
                    ShareTools.ShareMusic(tag.Music);
                    break;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            foreach (var item in this.flyoutItems) item.Click -= this.FlyoutItem_Click;

            this.flyoutItems.Clear();
            this.DataList.ItemsSource = null;
            this.albums = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            this.load();
            DataList.ItemsSource = this.ItemsSource;
        }

        private void TextBlock_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args) {
            if (args.NewValue == null) return;
            ( sender as TextBlock ).Text = (DataList.IndexFromContainer(DataList.ContainerFromItem(args.NewValue)) + 1).ToString();
        }
        private void FlyoutAddTo_Loaded(object sender, RoutedEventArgs e) {
            var item = sender as MenuFlyoutSubItem;
            if (item.Items.Count > 2) return;

            var likeItem = new MenuFlyoutItem() { Text = "我喜欢的音乐", Icon = new FontIcon() { Glyph = "\uEB51", FontFamily = new FontFamily("Segoe MDL2 Assets") }, Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Like, Music = item.Tag as Music } };
            likeItem.Click += this.FlyoutItem_Click;
            item.Items.Add(likeItem);

            foreach (var album in this.albums) {
                var albumItem = new MenuFlyoutItem() { Icon = new SymbolIcon(Symbol.MusicInfo), Text = album.name, Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Add, AlbumId = album.id, Music = item.Tag as Music } };
                albumItem.Click += this.FlyoutItem_Click;

                item.Items.Add(albumItem);
            }
        }

        private void FlyoutNext_Click(object sender, RoutedEventArgs e) {
            var tag = ( sender as MenuFlyoutItem ).Tag as Music;
            if (!App.Player.PlayList.Contains(tag)) {
                int index = 0;
                if (App.Player.PlayList.Contains(App.Player.NowPlayingMusic)) index = App.Player.PlayList.IndexOf(App.Player.NowPlayingMusic) + 1;
                App.Player.PlayList.Insert(index, tag);

                if (App.Player.NowPlayingMusic == null) App.Player.SetMusic(App.Player.PlayList[0]);
            } else {
                InfoBarPopup.Show("添加失败", "歌曲已存在", InfoBarSeverity.Error);
            }
        }

        private void FlyoutArtist_Loaded(object sender, RoutedEventArgs e) {
            var item = sender as MenuFlyoutSubItem;
            var tag = item.Tag as Music;

            item.Items.Clear();
            foreach (var temp in tag.artists) {
                var artistItem = new MenuFlyoutItem() { Text = temp.name.origin, Tag = temp };
                artistItem.Click += this.ArtistItem_Click;
                item.Items.Add(artistItem);
            }
        }

        private void ArtistItem_Click(object sender, RoutedEventArgs e) =>
            App.ViewModel.NavigateToPage(typeof(Pages.Artist), ( sender as MenuFlyoutItemBase ).Tag as Artist);
    }

    /// <summary>
    /// 音乐列表 Item Flyout
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(FlyoutItemTag))]
    public class FlyoutItemTag : MarkupExtension {
        protected override object ProvideValue() => this;

        /// <summary>
        /// Flyout Item 操作
        /// </summary>
        public FlyoutItemMode Mode { get; set; }
        /// <summary>
        /// 歌单 id
        /// </summary>
        public string AlbumId { get; set; }
        /// <summary>
        /// 音乐 Music Object
        /// </summary>
        public Music Music { get; set; }
    }

    public enum FlyoutItemMode {
        /// <summary>
        /// 添加到歌单
        /// </summary>
        Add,
        /// <summary>
        /// 添加到下一曲播放
        /// </summary>
        AddNextPlay,
        /// <summary>
        /// 喜欢音乐
        /// </summary>
        Like,
        /// <summary>
        /// 分享音乐
        /// </summary>
        Share,
        /// <summary>
        /// 播放音乐
        /// </summary>
        Play,
        /// <summary>
        /// 从歌单删除
        /// </summary>
        Remove
    }
}