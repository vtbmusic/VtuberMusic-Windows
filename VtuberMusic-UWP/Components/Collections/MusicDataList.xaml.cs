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
using Windows.UI.Xaml.Media;

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
                Tag = new FlyoutItemTag { Mode = FlyoutItemMode.AddNextPlay, Music = music },
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
                    Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Add, AlbumId = item.id, Music = music }
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
            switch (tag.Mode)
            {
                case FlyoutItemMode.AddNextPlay:
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

                    break;
                case FlyoutItemMode.Add:
                    try
                    {
                        await App.Client.Account.TrackMusic(tag.AlbumId, Service.TrackType.add, new string[] { tag.Music.id });
                        InfoBarPopup.Show("添加成功", "", InfoBarSeverity.Success);
                    }
                    catch (Exception ex)
                    {
                        InfoBarPopup.Show("无法添加到歌单", ex.Message, InfoBarSeverity.Error);
                    }
                    break;
                case FlyoutItemMode.Play:
                    App.Player.SetMusic(tag.Music);
                    break;
                case FlyoutItemMode.Like:
                    try
                    {
                        await App.Client.Account.LikeMusic(tag.Music.id);
                        InfoBarPopup.Show("喜欢成功", "", InfoBarSeverity.Success);
                    }
                    catch (Exception ex)
                    {
                        InfoBarPopup.Show("喜欢音乐失败", ex.Message, InfoBarSeverity.Error);
                    }
                    break;
                case FlyoutItemMode.Share:
                    ShareTools.ShareMusic(tag.Music);
                    break;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in flyoutItems) item.Click -= FlyoutItem_Click;

            flyoutItems.Clear();
            DataList.ItemsSource = null;
            albums = null;
        }

        private void UserControl_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var control = sender as UserControl;
            var tag = control.Tag as Music;
            var flyout = new MenuFlyout() { Placement = FlyoutPlacementMode.Bottom };

            var playItem = new MenuFlyoutItem { Icon = new SymbolIcon(Symbol.Play), Text = "播放", Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Play, Music = tag } };
            playItem.Click += FlyoutItem_Click;

            var addItem = new MenuFlyoutSubItem() { Icon = new SymbolIcon(Symbol.Add), Text = "添加到..." };
            var nextPlayItem = new MenuFlyoutItem()
            {
                Icon = new SymbolIcon(Symbol.Next),
                Text = "下一首播放",
                Tag = new FlyoutItemTag { Mode = FlyoutItemMode.AddNextPlay, Music = tag }
            };

            nextPlayItem.Click += FlyoutItem_Click;
            addItem.Items.Add(nextPlayItem);
            addItem.Items.Add(new MenuFlyoutSeparator());

            var likeItem = new MenuFlyoutItem() { Text = "我喜欢的音乐", Icon = new FontIcon() { Glyph = "\uEB51", FontFamily = new FontFamily("Segoe MDL2 Assets") }, Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Like, Music = tag } };
            likeItem.Click += FlyoutItem_Click;
            addItem.Items.Add(likeItem);

            foreach (var album in albums)
            {
                var item = new MenuFlyoutItem() { Icon = new SymbolIcon(Symbol.MusicInfo), Text = album.name, Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Add, AlbumId = album.id, Music = tag } };
                item.Click += FlyoutItem_Click;

                addItem.Items.Add(item);
            }

            var shareItem = new MenuFlyoutItem() { Icon = new SymbolIcon(Symbol.Share), Text = "分享", Tag = new FlyoutItemTag { Mode = FlyoutItemMode.Share, Music = tag } };
            shareItem.Click += FlyoutItem_Click;

            flyout.Items.Add(playItem);
            flyout.Items.Add(new MenuFlyoutSeparator());
            flyout.Items.Add(addItem);
            flyout.Items.Add(shareItem);
            flyout.Items.Add(new MenuFlyoutSeparator());

            control.ContextFlyout = flyout;
        }
    }

    public class FlyoutItemTag
    {
        public FlyoutItemMode Mode { get; set; }
        public string AlbumId { get; set; }
        public Music Music { get; set; }
    }

    public enum FlyoutItemMode
    {
        Add,
        AddNextPlay,
        Like,
        Share,
        Play,
        Remove
    }
}