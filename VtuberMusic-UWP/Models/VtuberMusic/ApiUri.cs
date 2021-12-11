namespace VtuberMusic_UWP.Models.VtuberMusic {
    /// <summary>
    /// Api 地址定义
    /// </summary>
    public class ApiUri {
        /// <summary>
        /// api 基本 url
        /// </summary>
        public static string BaseUrl = "https://api.aqua.chat";

        /// <summary>
        /// 获取最新音乐列表
        /// </summary>
        public static string NewSong = BaseUrl + "/v2/song/news";

        /// <summary>
        /// 获取音乐 media 文件 url
        /// </summary>
        public static string SongMedia = BaseUrl + "/v2/song/url/media/";

        /// <summary>
        /// 获取 banner
        /// </summary>
        public static string Banner = BaseUrl + "/v2/banner";

        /// <summary>
        /// 获取 Vtuber 列表
        /// </summary>
        public static string ArtistList = BaseUrl + "/v2/artist/list";

        /// <summary>
        /// 获取歌单列表
        /// </summary>
        public static string PlayListList = BaseUrl + "/v2/playlist/list";

        /// <summary>
        /// 获取歌单内音乐列表
        /// </summary>
        public static string PlayListSongs = BaseUrl + "/v2/playlist/song";

        /// <summary>
        /// 获取 Vtuber 音乐列表
        /// </summary>
        public static string ArtistSongs = BaseUrl + "/v2/artist/songs";

        /// <summary>
        /// 搜索
        /// </summary>
        public static string Search = BaseUrl + "/v2/search";

        /// <summary>
        /// 登录
        /// </summary>
        public static string Login = BaseUrl + "/v2/user/login";

        /// <summary>
        /// 获取账户信息
        /// </summary>
        public static string AccountInfo = BaseUrl + "/v2/user/account";

        /// <summary>
        /// 获取该账户创建的歌单列表
        /// </summary>
        public static string MyCreatePlayList = BaseUrl + "/v2/playlist/createlist";

        /// <summary>
        /// 获取该账户收藏的歌单列表
        /// </summary>
        public static string MyFavouitePlayList = BaseUrl + "/v2/playlist/sublist";

        /// <summary>
        /// 获取该账户喜欢的音乐列表
        /// </summary>
        public static string GetUserLikeMusic = BaseUrl + "/v2/playlist/likelist";

        /// <summary>
        /// 喜欢音乐
        /// </summary>
        public static string LikeMusic = BaseUrl + "/v2/song/like";

        /// <summary>
        /// 获取 4 个项目的数量
        /// </summary>
        public static string SubCount = BaseUrl + "/v2/user/subcount";

        /// <summary>
        /// 创建歌单
        /// </summary>
        public static string CreateAlbum = BaseUrl + "/v2/playlist/create";

        /// <summary>
        /// 增 / 删音乐到歌单
        /// </summary>
        public static string TrackMusic = BaseUrl + "/v2/playlist/tracks";

        /// <summary>
        /// 编辑歌单信息
        /// </summary>
        public static string EditAlbum = BaseUrl + "/v2/playlist/update";

        /// <summary>
        /// 获取播放记录
        /// </summary>
        public static string GetRecord = BaseUrl + "/v2/user/record";

        /// <summary>
        /// 收藏歌单
        /// </summary>
        public static string SubscribeAlbum = BaseUrl + "/v2/playlist/subscribe";

        /// <summary>
        /// 获取个人资料
        /// </summary>
        public static string GetProfile = BaseUrl + "/v2/user/detail/";

        public static string GetMusicComments = BaseUrl + "/v3/comment/song";

        public static string GetPersonalizedMusic = BaseUrl + "/v3/personalized/song";
    }
}
