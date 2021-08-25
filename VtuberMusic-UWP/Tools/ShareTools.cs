using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace VtuberMusic_UWP.Tools
{
    public class ShareTools
    {
        public static void ShareMusic(Music data)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            DataTransferManager.ShowShareUI();

            dataTransferManager.DataRequested += delegate (DataTransferManager s, DataRequestedEventArgs args)
            {
                args.Request.Data.SetWebLink(new Uri("https://vtbmusic.com/song?id=" + data.id));
                args.Request.Data.SetText(data.name + " - " + UsefullTools.GetArtistsString(data.artists));

                args.Request.Data.Properties.Title = data.name;
                args.Request.Data.Properties.Description = UsefullTools.GetArtistsString(data.artists);
                args.Request.Data.Properties.ApplicationName = "VtuberMusic";
                args.Request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(data.picUrl));
            };
        }
    }
}
