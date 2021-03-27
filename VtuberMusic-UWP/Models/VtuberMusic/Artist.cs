using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Artist
    {
        public string id { get; set; }
        public object uid { get; set; }
        public string imgUrl { get; set; }
        public Artistname name { get; set; }
        public string groupId { get; set; }
        public string groupName { get; set; }
        public object picUrl { get; set; }
        public bool followed { get; set; }
        public int musicSize { get; set; }
        public int albumSize { get; set; }
        public int likeSize { get; set; }
    }

    public class Artistname
    {
        public string origin { get; set; }
        public string cn { get; set; }
        public string jp { get; set; }
        public string en { get; set; }
    }
}
