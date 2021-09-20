using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic.v1 {
    public class Comment {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string MusicId { get; set; }
        public string Level { get; set; }
        public string Text { get; set; }
        public string Avatar { get; set; }
        public string CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public string[] RoleNames { get; set; }
        public Comment[] Children { get; set; }
        public int Total { get; set; }
        public bool isLike { get; set; }
    }
}
