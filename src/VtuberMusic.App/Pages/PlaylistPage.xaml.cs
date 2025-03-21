﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.Controls.DataItem;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.ViewModels.Pages;
using VtuberMusic.AppCore.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class PlaylistPage : Page {
    private readonly PlaylistPageViewModel ViewModel = Ioc.Default.GetRequiredService<PlaylistPageViewModel>();

    public PlaylistPage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        var arg = e.Parameter as PlaylistPageArg;
        ViewModel.Playlist = arg.Playlist;
        ViewModel.PlaylistType = arg.PlaylistType;
    }

    private void MusicListItem_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        var item = sender as MusicListItem;
        item.PlaylistId = ViewModel.Playlist.id;
        item.CanRemove = ViewModel.CanRemoveMusic;
        item.IsFavoriteMusic = ViewModel.PlaylistType == PlaylistType.LikeMusics;
    }

    private void PageRoot_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        ViewModel.IsActive = true;
    }

    private void PageRoot_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        ViewModel.IsActive = false;
    }

    private async void DeletePlaylistAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        await new ConfirmDeletePlaylistDialog().ShowDialogAsync(ViewModel.Playlist);

    private async void EditPlaylistAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        await new EditPlaylistInfoDialog().ShowDialogAsync(ViewModel.Playlist);
}
