using PropertyChanged;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    /// <summary>
    /// 账户 Object
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class Account {
        /// <summary>
        /// 账户 id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 账户 userName
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 账户注册时间
        /// </summary>
        public int createTime { get; set; }
        /// <summary>
        /// 账户封禁状态
        /// </summary>
        public BanStatue ban { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public int lastLoginTime { get; set; }
        /// <summary>
        /// 最后登录 ip
        /// </summary>
        public string lastLoginIP { get; set; }
    }

    /// <summary>
    /// 账户 4 个项目的 Object
    /// </summary>
    public class AccountSubCount {
        /// <summary>
        /// 主推数量
        /// </summary>
        public int artistCount { get; set; }
        /// <summary>
        /// 喜欢的音乐数量
        /// </summary>
        public int songCount { get; set; }
        /// <summary>
        /// 创建的歌单数量
        /// </summary>
        public int createdPlaylistCount { get; set; }
        /// <summary>
        /// 收藏的歌单数量
        /// </summary>
        public int subPlaylistCount { get; set; }
    }

    /// <summary>
    /// 封禁状态
    /// </summary>
    public enum BanStatue {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 封禁中
        /// </summary>
        Ban
    }
}
