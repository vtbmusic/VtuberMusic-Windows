using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Artist
    {
        public string Id { get; set; }
        public string CreateTime { get; set; }
        public string CreatorId { get; set; }
        public bool Deleted { get; set; }
        public string OriginalName { get; set; }
        public string ChineseName { get; set; }
        public string JapaneseName { get; set; }
        public string EnglistName { get; set; }
        public string GroupsId { get; set; }
        public string AvatarImg { get; set; }
        public string Bilibili { get; set; }
        public string YouTube { get; set; }
        public string Twitter { get; set; }
        public string Language { get; set; }
        public object introduce { get; set; }
        public bool isLike { get; set; }
        public int Watchs { get; set; }
    }

    public class Name
    {
        public string origin { get; set; }
        public string cn { get; set; }
        public string jp { get; set; }
        public string en { get; set; }
    }

    public class VocalDataResponse
    {
        public int Total { get; set; }
        public Artist[] Data { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public object Msg { get; set; }
    }

    public class Vocallist
    {
        public string Id { get; set; }
        public string cn { get; set; }
        public string jp { get; set; }
        public string en { get; set; }
        public string originlang { get; set; }
    }
}
