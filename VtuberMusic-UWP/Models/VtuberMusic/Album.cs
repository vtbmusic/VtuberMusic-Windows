using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Album
    {
        public string id { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public int userId { get; set; }
        public int createTime { get; set; }
        public int updateTime { get; set; }
        public int subscribedCount { get; set; }
        public string coverImgUrl { get; set; }
        public int coverImgId { get; set; }
        public string description { get; set; }
        public object tags { get; set; }
        public int playCount { get; set; }
        public int trackUpdateTime { get; set; }
        public Profile creator { get; set; }
        public object subscribers { get; set; }
        public bool subscribed { get; set; }
        public int privacy { get; set; }
        public string recommendInfo { get; set; }
        public int shareCount { get; set; }
        public int commentCount { get; set; }
        public bool like { get; set; }
    }
}
