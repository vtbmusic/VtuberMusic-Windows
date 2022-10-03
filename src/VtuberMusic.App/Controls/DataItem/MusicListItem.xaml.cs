using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Messages;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.ViewModels.Controls.DataItem;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class MusicListItem : UserControl {
    public static readonly DependencyProperty MusicProperty =
        DependencyProperty.Register("Music", typeof(Music), typeof(MusicListItem), new PropertyMetadata(null, new PropertyChangedCallback(MusicPropertyChanged)));
    public static readonly DependencyProperty PlaylistIdProperty =
        DependencyProperty.Register("PlaylistId", typeof(string), typeof(MusicListItem), new PropertyMetadata(null));
    public static readonly DependencyProperty CanRemoveMusicProperty =
        DependencyProperty.Register("CanRemove", typeof(bool), typeof(MusicListItem), new PropertyMetadata(false, OnCanRemoveChanged));
    public static readonly DependencyProperty IsFavoriteMusicProperty =
        DependencyProperty.Register("IsFavoriteMusic", typeof(bool), typeof(MusicListItem), new PropertyMetadata(false, OnIsFavoriteMusicChanged));

    private readonly MusicDataItemViewModel ViewModel = Ioc.Default.GetRequiredService<MusicDataItemViewModel>();

    public Music Music {
        get => GetValue(MusicProperty) as Music;
        set => SetValue(MusicProperty, value);
    }

    public string PlaylistId {
        get => (string)GetValue(PlaylistIdProperty);
        set => SetValue(PlaylistIdProperty, value);
    }

    public bool CanRemove {
        get => (bool)GetValue(CanRemoveMusicProperty);
        set => SetValue(CanRemoveMusicProperty, value);
    }

    public bool IsFavoriteMusic {
        get => (bool)GetValue(IsFavoriteMusicProperty);
        set => SetValue(IsFavoriteMusicProperty, value);
    }

    public MusicListItem() {
        InitializeComponent();
    }

    private static void OnCanRemoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        (d as MusicListItem).ViewModel.CanRemove = (bool)e.NewValue;

    private static void OnIsFavoriteMusicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        (d as MusicListItem).ViewModel.IsFavoriteMusic = (bool)e.NewValue;

    private static void MusicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var musicItem = d as MusicListItem;
        musicItem.ArtistMenuFlyoutItem.Items.Clear();

        foreach (var item in musicItem.Music.artists) {
            musicItem.ArtistMenuFlyoutItem.Items.Add(new MenuFlyoutItem { Text = item.name.origin, Command = musicItem.ViewModel.NavigateToArtistCommand, CommandParameter = item });
        }
    }

    private async void TrackMusicMenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
        await new TrackMusicDialog().ShowDialogAsync(new string[] { this.Music.id });
    }

    private async void DeleteMenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
        await ViewModel.RemoveMusicFromPlaylist(this.PlaylistId, new string[] { this.Music.id }, this.IsFavoriteMusic);
        WeakReferenceMessenger.Default.Send(new PlaylistMusicChangedMessage(this.PlaylistId));
    }

    private void ShowCommentMenuFlyoutItem_Click(object sender, RoutedEventArgs e) =>
        NavigationHelper.Navigate<CommentPage>(new CommentPageArg { Type = CommentContentType.song, Music = this.Music });
}
