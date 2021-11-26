using PropertyChanged;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    /// <summary>
    /// 歌手
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class Artist {
        /// <summary>
        /// 歌手 id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 歌手的用户 id
        /// </summary>
        public object uid { get; set; }
        /// <summary>
        /// 歌手头像 url
        /// </summary>
        public string imgUrl { get; set; }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public Artistname name { get; set; }
        /// <summary>
        /// 歌手所属企划 id
        /// </summary>
        public string groupId { get; set; }
        /// <summary>
        /// 歌手所属企划名称
        /// </summary>
        public string groupName { get; set; }
        /// <summary>
        /// 歌手背景图 url
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 是否已关注
        /// </summary>
        public bool followed { get; set; }
        /// <summary>
        /// 歌曲数量
        /// </summary>
        public int musicSize { get; set; }
        /// <summary>
        /// 歌单数量
        /// </summary>
        public int albumSize { get; set; }
        /// <summary>
        /// 粉丝数量
        /// </summary>
        public int likeSize { get; set; }
    }

    /// <summary>
    /// 歌手名称
    /// </summary>
    public class Artistname {
        /// <summary>
        /// 歌手名称源文
        /// </summary>
        public string origin { get; set; }
        /// <summary>
        /// 歌手简体中文名称
        /// </summary>
        public string cn { get; set; }
        /// <summary>
        /// 歌手日文名称
        /// </summary>
        public string jp { get; set; }
        /// <summary>
        /// 歌手英文名称
        /// </summary>
        public string en { get; set; }
    }
}
