using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace VtuberMusic.Core.Models {
    public class Music {
        public string id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string picUrl { get; set; }
        public string vrcUrl { get; set; }
        public bool like { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset time { get; set; }
        public double? duration { get; set; }
        public Artist[] artists { get; set; }
        public MusicStatis statis { get; set; }
    }

    public class MusicStatis {
        public int playCount { get; set; }
        public int commentCount { get; set; }
        public int likeCount { get; set; }
        public int shareCount { get; set; }
    }
}
