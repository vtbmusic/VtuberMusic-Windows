using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Banner
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public bool Deleted { get; set; }
        public string CreatorRealName { get; set; }
        public string OriginName { get; set; }
        public string BannerImg { get; set; }
        public string Url { get; set; }
    }
}
