using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace VtuberMusic.Core.Models {
    public class Comment {
        public string id { get; set; }
        public Profile user { get; set; }
        public int status { get; set; }
        public string content { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset time { get; set; }
        public Comment[] beReplieds { get; set; }
        public int beRepliedCount { get; set; }
        public int likedCount { get; set; }
        public string parentCommentId { get; set; }
        public bool liked { get; set; }
        public bool unliked { get; set; }
    }
}
