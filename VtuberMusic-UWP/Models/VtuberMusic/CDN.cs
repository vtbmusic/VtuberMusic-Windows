using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class CDN
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string info { get; set; }
    }

    public class CDNDataResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
        public CDN[] Data { get; set; }
        public int Total { get; set; }
    }
}
