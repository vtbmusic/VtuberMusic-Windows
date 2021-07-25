namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Account
    {
        public string id { get; set; }
        public string userName { get; set; }
        public int createTime { get; set; }
        public BanStatue ban { get; set; }
        public int lastLoginTime { get; set; }
        public string lastLoginIP { get; set; }
    }

    public class AccountSubCount
    {
        public int artistCount { get; set; }
        public int songCount { get; set; }
        public int createdPlaylistCount { get; set; }
        public int subPlaylistCount { get; set; }
    }

    public enum BanStatue
    {
        Normal,
        Ban
    }
}
