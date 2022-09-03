﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Models;
using Windows.ApplicationModel.DataTransfer;

namespace VtuberMusic.App.ViewModels;
public partial class AppViewModel : ObservableRecipient {
    public AppViewModel() {
        #region Navigation
        NavigateToArtistCommand = new RelayCommand<Artist>((Artist artist) => {
            NavigationService.Navigate<ArtistPage>(new ArtistPageArg { Artist = artist });
        });

        NavigateToProfileCommand = new RelayCommand<Profile>((Profile profile) => {
            NavigationService.Navigate<ProfilePage>(new ProfilePageArg { Profile = profile });
        });

        NavigateToSearchCommand = new RelayCommand<string>((string keyword) => {
            if (string.IsNullOrWhiteSpace(keyword)) {
                return;
            }

            NavigationService.Navigate<Search>(new SearchPageArg { Keyword = keyword });
        });
        #endregion

        CopyLinkCommand = new RelayCommand<object>((object arg) => {
            DataPackage data = new();
            if (arg is Music) {
                data.SetText($"https://vtbmusic.com/song?id={(arg as Music).id}");
            } else if (arg is Artist) {
                data.SetText($"https://vtbmusic.com/vtuber?id={(arg as Artist).id}");
            } else if (arg is Playlist) {
                data.SetText($"https://vtbmusic.com/songlist?id={(arg as Playlist).id}");
            }

            Clipboard.SetContent(data);
        });

        ShareCommand = new RelayCommand<object>((object arg) => {
            if (arg is Music) {
                ShareHelper.ShareMusic(arg as Music);
            } else if (arg is Artist) {
                ShareHelper.ShareArtist(arg as Artist);
            } else if (arg is Playlist) {
                ShareHelper.SharePlaylist(arg as Playlist);
            }
        });

        SetNextMusicCommand = new RelayCommand<Music>((Music music) => MediaPlaybackService.SetNextMusic(music));
        SetCollectionCommand = new RelayCommand<IEnumerable<Music>>((IEnumerable<Music> collection) => MediaPlaybackService.SetCollection(collection));
        SetMusicCommand = new RelayCommand<Music>((Music music) => {
            MediaPlaybackService.SetMusic(music);
        });
    }
}
