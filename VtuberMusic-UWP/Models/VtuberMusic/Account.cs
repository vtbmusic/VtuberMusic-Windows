using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Account
    {
        public string id { get; set; }
        public string userName { get; set; }
        public int createTime { get; set; }
        public BanStatue ban { get; set; }
        public int lastLoginTime { get; set; }
        public string lastLoginIP { get; set; }
    }

    public enum BanStatue
    {
        Normal,
        Ban
    }
}
