﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Messages;
using VtuberMusic.App.Models;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class MainPageViewModel : ObservableRecipient {
    private readonly IVtuberMusicService _vtuberMusicService;

    public MainPageViewModel(IVtuberMusicService vtuberMusicService) {
        _vtuberMusicService = vtuberMusicService;
    }

    [ObservableProperty]
    private bool isPlayingShow;

    [ObservableProperty]
    private double pageHeight;

    [ObservableProperty]
    private ObservableCollection<NavigationViewItemBase> navigationItems;

    [ObservableProperty]
    private ObservableCollection<NavigationViewItemBase> paneFooterNavigationItems = new ObservableCollection<NavigationViewItemBase>
    {
        createNavgationItem(typeof(Library), "音乐库", new SymbolIcon(Symbol.Library)),
        createNavgationItem(typeof(NoticePage), "通知", new SymbolIcon(Symbol.Message))
    };

    protected override void OnActivated() =>
        WeakReferenceMessenger.Default.Register(this, async (object sender, UserPlaylistsChangedMessage message) => {
            await LoadNavigation(true);
        });

    [RelayCommand]
    public void NavigateToSearch(string keyword) {
        if (string.IsNullOrWhiteSpace(keyword)) return;

        NavigationHelper.Navigate<Search>(new SearchPageArg { Keyword = keyword });
    }

    [RelayCommand]
    public async Task Load() {
        await LoadNavigation();
    }

    public async Task LoadNavigation(bool reload = false) {
        this.NavigationItems = new ObservableCollection<NavigationViewItemBase>
{
            createNavgationItem(typeof(Discover), "发现", new SymbolIcon(Symbol.View)),
            new NavigationViewItemHeader { Content = "我的音乐" },
            //createNavgationItem(typeof(Discover), "历史播放", new SymbolIcon(Symbol.Clock))
        };

        if (!reload) {
            switch (SettingsHelper.DefaultNavigationPage) {
                case DefaultNavigationPage.Home:
                    NavigationHelper.Navigate<Discover>();
                    break;
                case DefaultNavigationPage.Library:
                    NavigationHelper.Navigate<Library>();
                    break;
            }
        }

        var likeMusicsPlaylist = await _vtuberMusicService.GetFavouriteMusicsPlaylist();
        this.NavigationItems.Insert(2, createNavgationItem(typeof(PlaylistPage), "我喜欢的音乐", new FontIcon { FontFamily = new FontFamily("Segoe Fluent Icons"), Glyph = "\uE006" },
            new PlaylistPageArg { Playlist = likeMusicsPlaylist.Data.playlist, PlaylistType = PlaylistType.LikeMusics }));

        if (!reload) {
            if (SettingsHelper.DefaultNavigationPage == DefaultNavigationPage.LikeMusic)
                NavigationHelper.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = likeMusicsPlaylist.Data.playlist, PlaylistType = PlaylistType.LikeMusics });
        }

        this.NavigationItems.Add(new NavigationViewItemHeader { Content = "我创建的歌单" });
        var createPlaylists = (await _vtuberMusicService.GetCreatePlaylist()).Data;
        foreach (var item in createPlaylists) {
            this.NavigationItems.Add(createNavgationItem(typeof(PlaylistPage), item.name, new SymbolIcon(Symbol.MusicInfo), new PlaylistPageArg { Playlist = item }));
        }

        this.NavigationItems.Add(new NavigationViewItemHeader { Content = "我收藏的歌单" });
        var subPlaylists = (await _vtuberMusicService.GetSubPlaylist()).Data;
        foreach (var item in subPlaylists) {
            this.NavigationItems.Add(createNavgationItem(typeof(PlaylistPage), item.name, new SymbolIcon(Symbol.MusicInfo), new PlaylistPageArg { Playlist = item }));
        }
    }

    private static NavigationViewItem createNavgationItem(Type type, string title, IconElement icon, object args = null) => new() { Icon = icon, Content = title, Tag = new NavigationTag { Type = type, Args = args } };
}