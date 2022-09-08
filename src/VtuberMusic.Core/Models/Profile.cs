using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using VtuberMusic.Core.Enums;

namespace VtuberMusic.Core.Models {
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
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset createTime { get; set; }
        public string userName { get; set; }
        public int accountType { get; set; }
        public int birthday { get; set; }
        public ProfileGender gender { get; set; }
        public int authStatus { get; set; }
        public string description { get; set; }
        public bool followed { get; set; }
        public bool allfollowed { get; set; }
        public int followeds { get; set; }
        public int fans { get; set; }
        public string remarkName { get; set; }
    }
}
