using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace VtuberMusic.Core.Models {
    public class Playlist {
        public string id { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public int userId { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset createTime { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset? updateTime { get; set; }
        public int subscribedCount { get; set; }
        public string coverImgUrl { get; set; }
        public int coverImgId { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
        public int playCount { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset? trackUpdateTime { get; set; }
        public Profile creator { get; set; }
        public string[] subscribers { get; set; }
        public bool subscribed { get; set; }
        public int privacy { get; set; }
        public string recommendInfo { get; set; }
        public int shareCount { get; set; }
        public int commentCount { get; set; }
        public bool like { get; set; }
        public int trackCount { get; set; }
    }
}
