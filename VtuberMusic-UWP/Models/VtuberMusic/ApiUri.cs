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

        public static string ArtistList = BaseUrl + "/v2/artist/list";

        public static string PlayListList = BaseUrl + "/v2/playlist/list";

        public static string PlayListSongs = BaseUrl + "/v2/playlist/song";

        public static string ArtistSongs = BaseUrl + "/v2/artist/songs";

        public static string Search = BaseUrl + "/v2/search";

        public static string Login = BaseUrl + "/v2/user/login";

        public static string AccountInfo = BaseUrl + "/v2/user/account";

        public static string MyCreatePlayList = BaseUrl + "/v2/playlist/createlist";

        public static string MyFavouitePlayList = BaseUrl + "/v2/playlist/sublist";

        public static string GetUserLikeMusic = BaseUrl + "/v2/playlist/likelist";

        public static string LikeMusic = BaseUrl + "/v2/song/like";

        public static string SubCount = BaseUrl + "/v2/user/subcount";

        public static string CreateAlbum = BaseUrl + "/v2/playlist/create";
    }
}
