namespace VtuberMusic_UWP.Models.VtuberMusic {
    /// <summary>
    /// 歌单 Object
    /// </summary>
    public class Album {
        /// <summary>
        /// 歌单 id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 歌单名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 歌单状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建者账户 id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public int createTime { get; set; }
        /// <summary>
        /// 歌单信息最后更新时间
        /// </summary>
        public int updateTime { get; set; }
        /// <summary>
        /// 被收藏数量
        /// </summary>
        public int subscribedCount { get; set; }
        /// <summary>
        /// 封面图 Url string
        /// </summary>
        public string coverImgUrl { get; set; }
        /// <summary>
        /// 封面图图片 id
        /// </summary>
        public int coverImgId { get; set; }
        /// <summary>
        /// 歌单描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 歌单 tag
        /// </summary>
        public string[] tags { get; set; }
        /// <summary>
        /// 被播放次数
        /// </summary>
        public int playCount { get; set; }
        /// <summary>
        /// 歌单内歌曲最后更新时间
        /// </summary>
        public int trackUpdateTime { get; set; }
        /// <summary>
        /// 创建者 Profile Object
        /// </summary>
        public Profile creator { get; set; }
        /// <summary>
        /// 收藏者
        /// </summary>
        public object subscribers { get; set; }
        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool subscribed { get; set; }
        /// <summary>
        /// 是否为隐私歌单
        /// </summary>
        public int privacy { get; set; }
        /// <summary>
        /// 推荐信息
        /// </summary>
        public string recommendInfo { get; set; }
        /// <summary>
        /// 被分享数量
        /// </summary>
        public int shareCount { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int commentCount { get; set; }
        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool like { get; set; }
    }

    /// <summary>
    /// 歌单歌曲 Object
    /// </summary>
    public class AlbumSong {
        /// <summary>
        /// 歌单信息
        /// </summary>
        public Album playlist { get; set; }
        /// <summary>
        /// 歌单内歌曲列表
        /// </summary>
        public Music[] songs { get; set; }
    }
}
