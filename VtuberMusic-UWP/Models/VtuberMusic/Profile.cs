namespace VtuberMusic_UWP.Models.VtuberMusic {
    public class Profile {
        public string userId { get; set; }
        public int userType { get; set; }
        public string nickname { get; set; }
        public int avatarImgId { get; set; }
        public string avatarUrl { get; set; }
        public int backgroundImgId { get; set; }
        public string backgroundUrl { get; set; }
        public string level { get; set; }
        public int? experience { get; set; }
        public int? nextexperience { get; set; }
        public string signature { get; set; }
        public int createTime { get; set; }
        public string userName { get; set; }
        public int accountType { get; set; }
        public int? birthday { get; set; }
        public int authStatus { get; set; }
        public string description { get; set; }
        public string remarkName { get; set; }
    }

    public class ProfileResponse {
        public Profile profile { get; set; }
    }
}
