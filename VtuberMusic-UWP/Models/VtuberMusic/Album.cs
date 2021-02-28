using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class AlbumData
    {
        public MusicData[] Data { get; set; }
        public bool isLike { get; set; }
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? PublishTime { get; set; }
        public string CreatorId { get; set; }
        public string CreatorRealName { get; set; }
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public string CoverImg { get; set; }
        public string introduce { get; set; }
        public object VocalId { get; set; }
        public object VocalName { get; set; }
        public string Form { get; set; }
        public string Type { get; set; }
    }

    public class GetAlbumDataRequeset
    {
        public string id { get; set; }
    }

    public class AlbumListResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
        public AlbumData[] Data { get; set; }
        public int Total { get; set; }
    }

    public class AlbumDataResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
        public AlbumData Data { get; set; }
        public int Total { get; set; }
    }
}
