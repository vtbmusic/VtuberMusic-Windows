using System;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace VtuberMusic_UWP.Tools {
    /// <summary>
    /// 分享工具
    /// </summary>
    public class ShareTools {
        /// <summary>
        /// 分享音乐
        /// </summary>
        /// <param name="data">音乐 Music Object</param>
        public static void ShareMusic(Music data) {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            DataTransferManager.ShowShareUI();

            dataTransferManager.DataRequested += delegate (DataTransferManager s, DataRequestedEventArgs args) {
                args.Request.Data.SetWebLink(new Uri("https://vtbmusic.com/song?id=" + data.id));
                args.Request.Data.SetText(data.name + " - " + UsefullTools.GetArtistsString(data.artists));

                args.Request.Data.Properties.Title = data.name;
                args.Request.Data.Properties.Description = UsefullTools.GetArtistsString(data.artists);
                args.Request.Data.Properties.ApplicationName = "VtuberMusic";
                args.Request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(data.picUrl));
            };
        }

        /// <summary>
        /// 分享歌单
        /// </summary>
        /// <param name="data">歌单 Album Object</param>
        public static void ShareAlbum(Album data) {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            DataTransferManager.ShowShareUI();

            dataTransferManager.DataRequested += delegate (DataTransferManager s, DataRequestedEventArgs args) {
                args.Request.Data.SetWebLink(new Uri("https://vtbmusic.com/songlist?id=" + data.id));
                args.Request.Data.SetText(data.name + " - " + data.creator.nickname + " 创建的歌单");

                args.Request.Data.Properties.Title = data.name;
                args.Request.Data.Properties.Description = data.creator.nickname + " 创建的歌单";
                args.Request.Data.Properties.ApplicationName = "VtuberMusic";
                args.Request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(data.coverImgUrl));
            };
        }
    }
}
