using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VtuberMusic.App.Models;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace VtuberMusic.App.ViewModels {
    public class MainPageViewModel : AppViewModel {
        private readonly IVtuberMusicService _vtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public bool IsPlayingShow { get => isPlayingShow; set => SetProperty(ref isPlayingShow, value); }
        private bool isPlayingShow;

        public double PageHeight { get => pageHeight; set => SetProperty(ref pageHeight, value); }
        private double pageHeight;

        public ObservableCollection<Microsoft.UI.Xaml.Controls.NavigationViewItemBase> NavigationItems { get; set; } = new ObservableCollection<Microsoft.UI.Xaml.Controls.NavigationViewItemBase>
        {
            createNavgationItem(typeof(Discover), "发现", new SymbolIcon(Symbol.View)),
            new Microsoft.UI.Xaml.Controls.NavigationViewItemHeader { Content = "我的音乐" },
            createNavgationItem(typeof(Discover), "历史播放", new SymbolIcon(Symbol.Clock))
        };

        public ObservableCollection<Microsoft.UI.Xaml.Controls.NavigationViewItemBase> PaneFooterNavigationItems { get; set; } = new ObservableCollection<Microsoft.UI.Xaml.Controls.NavigationViewItemBase>
        {
            createNavgationItem(typeof(Library), "音乐库", new SymbolIcon(Symbol.Library))
        };

        public MainPageViewModel() {
            LoadCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        private async Task LoadDataAsync() {
            var likeMusicsPlaylist = await _vtuberMusicService.GetFavouriteMusicsPlaylist();
            NavigationItems.Insert(2, createNavgationItem(typeof(PlaylistPage), "我喜欢的音乐", new FontIcon { FontFamily = new FontFamily("Segoe Fluent Icons"), Glyph = "\uE006" },
                new PlaylistPageArg { Playlist = likeMusicsPlaylist.Data.playlist, PlaylistType = PlaylistType.LikeMusics }));

            NavigationItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItemHeader { Content = "我创建的歌单" });
            var createPlaylists = (await _vtuberMusicService.GetCreatePlaylist()).Data;
            foreach (var item in createPlaylists) {
                NavigationItems.Add(createNavgationItem(typeof(PlaylistPage), item.name, new SymbolIcon(Symbol.MusicInfo), new PlaylistPageArg { Playlist = item }));
            }

            NavigationItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItemHeader { Content = "我收藏的歌单" });
            var subPlaylists = (await _vtuberMusicService.GetSubPlaylist()).Data;
            foreach (var item in subPlaylists) {
                NavigationItems.Add(createNavgationItem(typeof(PlaylistPage), item.name, new SymbolIcon(Symbol.MusicInfo), new PlaylistPageArg { Playlist = item }));
            }
        }

        private static Microsoft.UI.Xaml.Controls.NavigationViewItem createNavgationItem(Type type, string title, IconElement icon, object args = null)
            => new Microsoft.UI.Xaml.Controls.NavigationViewItem { Icon = icon, Content = title, Tag = new NavigationTag { Type = type, Args = args } };
    }
}