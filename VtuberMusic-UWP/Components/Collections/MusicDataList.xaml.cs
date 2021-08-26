using Microsoft.AppCenter.Crashes;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Tools;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;


namespace VtuberMusic_UWP.Components.Collections
{
    public partial class MusicDataList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Music[]), typeof(MusicDataList), new PropertyMetadata("ItemsSource", new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        public Music[] ItemsSource
        {
            get { return (Music[])GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Album[] albums = null;
        private List<MenuFlyoutItem> flyoutItems = new List<MenuFlyoutItem>();

        public MusicDataList()
        {
            this.InitializeComponent();
            load();
        }

        public async void load() => albums = (await App.Client.Account.GetMyCreatePlayList()).Data;
        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MusicDataList)d).ItemsSourceChange(e);

        private protected void ItemsSourceChange(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue.GetType() == typeof(Music[]))
            {
                DataList.ItemsSource = e.NewValue;
            }
            else
            {
                DataList.ItemsSource = null;
            }
        }

        private void DataList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (DataList.SelectedItem != null)
            {
                App.Player.SetMusic((Music)DataList.SelectedItem);
            }
        }

        private void Count_Loaded(object sender, RoutedEventArgs e)
        {
            var item = sender as TextBlock;

            for (int i = 0; i != ItemsSource.Length; i++)
            {
                if (item.Tag as Music == ItemsSource[i])
                {
                    item.Text = (i + 1).ToString();
                    return;
                }
            }
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
                VisualStateManager.GoToState(sender as Control, "Hover", true);
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e) =>
            VisualStateManager.GoToState(sender as Control, "Normal", true);

        private void ArtistButton_Click(object sender, RoutedEventArgs e) => App.ViewModel.NavigateToPage(typeof(Pages.Artist), ((HyperlinkButton)sender).Tag);
        private void Play_Click(object sender, RoutedEventArgs e) => App.Player.SetMusic((Music)(sender as Control).Tag);

        private async void Like_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var music = (Music)button.Tag;
            button.IsEnabled = false;

            try
            {
                await App.Client.Account.LikeMusic(music.id, !music.like);
                ((Music)button.Tag).like = !music.like;

                if (((Music)button.Tag).like)
                {
                    ((FontIcon)(button.Content)).Glyph = "\uE00B";
                }
                else
                {
                    ((FontIcon)(button.Content)).Glyph = "\uE006";
                }

                button.IsEnabled = true;
            }
            catch (Exception ex)
            {
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
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var music = (Music)button.Tag;
            var menuFlyout = new MenuFlyout() { Placement = FlyoutPlacementMode.Bottom };
            var nextItem = new MenuFlyoutItem
            {
                Icon = new SymbolIcon(Symbol.Next),
                Text = "下一首播放",
                Tag = new FlyoutItemTag { Playlist = true, Music = music },
            };

            nextItem.Click += FlyoutItem_Click;
            menuFlyout.Items.Add(nextItem);
            flyoutItems.Add(nextItem);
            menuFlyout.Items.Add(new MenuFlyoutSeparator());

            foreach (var item in albums)
            {
                var flyoutItem = new MenuFlyoutItem
                {
                    Text = item.name,
                    Icon = new SymbolIcon(Symbol.MusicInfo),
                    Tag = new FlyoutItemTag { AlbumId = item.id, Music = music }
                };

                flyoutItem.Click += FlyoutItem_Click;
                menuFlyout.Items.Add(flyoutItem);
                flyoutItems.Add(flyoutItem);
            }

            button.Flyout = menuFlyout;
        }

        private void Share_Click(object sender, RoutedEventArgs e) => ShareTools.ShareMusic((Music)((Control)sender).Tag);

        private async void FlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var tag = (FlyoutItemTag)((MenuFlyoutItem)sender).Tag;
            if (tag.Playlist)
            {
                if (!App.Player.PlayList.Contains(tag.Music))
                {
                    int index = 0;
                    if (App.Player.PlayList.Contains(App.Player.NowPlayingMusic)) index = App.Player.PlayList.IndexOf(App.Player.NowPlayingMusic) + 1;
                    App.Player.PlayList.Insert(index, tag.Music); ;

                    if (App.Player.NowPlayingMusic == null) App.Player.SetMusic(App.Player.PlayList[0]);
                }
                else
                {
                    InfoBarPopup.Show("添加失败", "歌曲已存在", InfoBarSeverity.Error);
                }
            }
            else
            {
                try
                {
                    await App.Client.Account.TrackMusic(tag.AlbumId, Service.TrackType.add, new string[] { tag.Music.id });
                    InfoBarPopup.Show("添加成功", "", InfoBarSeverity.Success);
                }
                catch (Exception ex)
                {
                    InfoBarPopup.Show("无法添加到歌单", ex.Message, InfoBarSeverity.Error);
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in flyoutItems) item.Click -= FlyoutItem_Click;

            flyoutItems.Clear();
            DataList.ItemsSource = null;
            albums = null;
        }
    }

    public class FlyoutItemTag
    {
        public bool Playlist { get; set; } = false;
        public string AlbumId { get; set; }
        public Music Music { get; set; }
    }
}