using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class Comment {
        public string id { get; set; }
        public Profile user { get; set; }
        public int status { get; set; }
        public string content { get; set; }
        public int time { get; set; }
        public Comment[] beReplieds { get; set; }
        public int beRepliedCount { get; set; }
        public int likedCount { get; set; }
        public string parentCommentId { get; set; }
        public bool liked { get; set; }
        public bool unliked { get; set; }
    }
}
