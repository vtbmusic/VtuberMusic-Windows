using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Service;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Music
    {
        public string id { get; set; }
        public string name { get; set; }
        public object alias { get; set; }
        public string picUrl { get; set; }
        public bool like { get; set; }
        public int time { get; set; }
        public float duration { get; set; }
        public Artist[] artists { get; set; }
        public MusicStatis statis { get; set; }
    }

    public class MusicStatis
    {
        public int playCount { get; set; }
        public int commentCount { get; set; }
        public int likeCount { get; set; }
        public int shareCount { get; set; }
    }
}
