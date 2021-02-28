using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Service;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class MusicData
    {
        public string Id { get; set; }
        public string CreateTime { get; set; }
        public string PublishTime { get; set; }
        public object CreatorId { get; set; }
        public object CreatorRealName { get; set; }
        public bool Deleted { get; set; }
        public string OriginName { get; set; }
        public string VocalId { get; set; }
        public string VocalName { get; set; }
        public bool IsPlaying
        {
            get
            {
                return App.Player.PlayList.All(m => m == this);
            }
        }
        private string _coverImg { get; set; }
        public string CoverImg
        {
            get
            {
                return _coverImg;
            }
            set
            {
                ResourcesUrl.CoverImg = value;
                _coverImg = value;
            }
        }

        private string _music { get; set; }
        public string Music
        {
            get
            {
                return _music;
            }
            set
            {
                ResourcesUrl.Music = value;
                _music = value;
            }
        }

        private string _lyrics { get; set; }
        public string Lyric
        {
            get
            {
                return _lyrics;
            }
            set
            {
                ResourcesUrl.Lyric = value;
                _lyrics = value;
            }
        }

        private string _cdn { get; set; }
        public string CDN
        {
            get
            {
                return _cdn;
            }
            set
            {
                _cdn = value;
                ResourcesUrl.CDN = value;
            }
        }

        public string BiliBili { get; set; }
        public string YouTube { get; set; }
        public string Twitter { get; set; }
        public int? Likes { get; set; }
        public double? Length { get; set; }
        public string Label { get; set; }
        public bool isLike { get; set; }
        public string Source { get; set; }
        public object SourceName { get; set; }
        public Vocallist[] VocalList { get; set; }
        public ResourcesUrl ResourcesUrl { get; set; } = new ResourcesUrl();
    }

    public class MusicDataResponse
    {
        public long Total { get; set; }
        public MusicData[] Data { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }

    public class ResourcesUrl
    {
        public string CDN { get; set; }
        private string _music { get; set; }
        public string Music
        {
            get
            {
                return App.Client.GetResourcesUrl(_music, CDN, ResourcesType.Music);
            }
            set
            {
                _music = value;
            }
        }

        private string _coverImg { get; set; }
        public string CoverImg
        {
            get
            {
                return App.Client.GetResourcesUrl(_coverImg, CDN, ResourcesType.CoverImg);
            }
            set
            {
                _coverImg = value;
            }
        }

        private string _lyric { get; set; }
        public string Lyric
        {
            get
            {
                return App.Client.GetResourcesUrl(_lyric, CDN, ResourcesType.Lyric);
            }
            set
            {
                _lyric = value;
            }
        }
    }
}
