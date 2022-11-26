using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace VtuberMusic.Core.Models {
    public class Notice {
        public string id { get; set; }
        public string title { get; set; }
        public string mssage { get; set; }
        public object imageUrl { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset time { get; set; }
        public bool queued { get; set; }
    }

    public class CommentNotice {
        public string id { get; set; }
        public string title { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset time { get; set; }
        public string createorId { get; set; }
        public string createorName { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string createorAvatarUrl { get; set; }
        public string commentId { get; set; }
        public string commenText { get; set; }
        public bool isLike { get; set; }
    }

    public class MessageCount {
        public int notice { get; set; }
        public int forward { get; set; }
        public int msg { get; set; }
        public int comment { get; set; }
    }
}
