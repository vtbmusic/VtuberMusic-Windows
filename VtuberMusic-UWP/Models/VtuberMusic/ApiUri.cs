using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class ApiUri
    {
        /// <summary>
        /// Api 地址
        /// </summary>
        public static string BaseUrl = "https://api.aqua.chat";

        public static string NewSong = BaseUrl + "/v2/song/news";

        public static string SongMedia = BaseUrl + "/v2/song/url/media/";

        public static string Banner = BaseUrl + "/v2/banner";
    }
}
