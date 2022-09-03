namespace VtuberMusic.Core.Models {
    public class Artist {
        public string id { get; set; }
        public string uid { get; set; }
        public string imgUrl { get; set; }
        public ArtistName name { get; set; }
        public string groupId { get; set; }
        public string groupName { get; set; }
        public string picUrl { get; set; }
        public bool followed { get; set; }
        public int musicSize { get; set; }
        public int albumSize { get; set; }
        public int likeSize { get; set; }
    }

    public class ArtistName {
        public string origin { get; set; }
        public string cn { get; set; }
        public string jp { get; set; }
        public string en { get; set; }
    }
}
