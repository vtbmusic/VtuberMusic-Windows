using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace VtuberMusic.AppCore.Helper {
    public class ShareHelper {
        public static void ShareMusic(Music music) {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
                var request = arg.Request;
                request.Data.SetWebLink(new Uri($"https://vtbmusic.com/song?id={ music.id }"));
                request.Data.Properties.ApplicationName = "VtuberMusic";
                request.Data.Properties.Title = music.name;
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(music.picUrl));
                request.Data.Properties.Description = $"{ music.name } - { MusicHelepr.GetArtistString(music.artists) }";
            };
            
            DataTransferManager.ShowShareUI();
        }

        public static void SharePlaylist(Playlist playlist) {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
                var request = arg.Request;
                request.Data.SetWebLink(new Uri($"https://vtbmusic.com/songlist?id={ playlist.id }"));
                request.Data.Properties.ApplicationName = "VtuberMusic";
                request.Data.Properties.Title = playlist.name;
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(playlist.coverImgUrl));
                request.Data.Properties.Description = $"{ playlist.name } - { playlist.creator.nickname }";
            };

            DataTransferManager.ShowShareUI();
        }

        public static void ShareArtist(Artist artist) {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
                var request = arg.Request;
                request.Data.SetWebLink(new Uri($"https://vtbmusic.com/vtuber?id={ artist.id }"));
                request.Data.Properties.ApplicationName = "VtuberMusic";
                request.Data.Properties.Title = artist.name.origin;
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(artist.imgUrl));
                request.Data.Properties.Description = $"{ artist.name.origin } - { artist.groupName }";
            };

            DataTransferManager.ShowShareUI();
        }
    }
}
