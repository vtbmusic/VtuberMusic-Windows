using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Tools;

namespace VtuberMusic_UWP.Service
{
    public class MusicClient
    {
        public AccountService Account = new AccountService();
        public CDN[] CDNList;

        public MusicClient(EventHandler loadCompleteEvent = null)
        {
            loadClient(loadCompleteEvent);
        }

        private async void loadClient(EventHandler loadCompleteEvent)
        {
            CDNList = (await GetCDNListAsync()).Data;

            if (loadCompleteEvent != null)
            {
                loadCompleteEvent(this, null);
            }

        }

        #region CDN 相关
        /// <summary>
        /// 获取资源 Url
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="cdn">CDN</param>
        /// <returns>返回资源 Url 文本</returns>
        public string GetResourcesUrl(string url, string cdn, ResourcesType type)
        {
            var cdnList = cdn.Split(':');
            if (cdnList.Length == 1)
            {
                return getResourcesUrlSingle(url, cdn);
            }
            else
            {
                switch (type)
                {
                    case ResourcesType.CoverImg:
                        return getResourcesUrlSingle(url, cdnList[0]);
                    case ResourcesType.Music:
                        return getResourcesUrlSingle(url, cdnList[1]);
                    case ResourcesType.Lyric:
                        return getResourcesUrlSingle(url, cdnList[2]);
                }
            }


            return null;
        }

        private string getResourcesUrlSingle(string url, string cdn)
        {
            foreach (var temp in CDNList)
            {
                if (temp.name == cdn)
                {
                    return temp.url + url;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取 CDN 列表
        /// </summary>
        /// <returns>CDN 数组 获取失败返回 null</returns>
        public async Task<CDNDataResponse> GetCDNListAsync()
        {
            var data =await NetworkTool.PostApiAsync<CDNDataResponse>(
                ApiUri.GetCDNList, new ListRequest { pageIndex = 1, pageRows = 1000, sortField = "name", sortType = "desc" });

            return data;
        }
        #endregion

        #region 音乐
        /// <summary>
        /// 获取音乐列表
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>Api 请求结果</returns>
        public async Task<MusicDataResponse> GetMusicListAsync(ListRequest args)
        {
            return await NetworkTool.PostApiAsync<MusicDataResponse>(ApiUri.GetMusicList, args);
        }

        /// <summary>
        /// 获取热门音乐列表
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>Api 请求结果</returns>
        public async Task<MusicDataResponse> GetHotMusicListAsync(ListRequest args)
        {
            return await NetworkTool.PostApiAsync<MusicDataResponse>(ApiUri.GetHotMusicList, args);
        }

        public async Task<MusicDataResponse> GetMusicDatasAsync(string[] ids)
        {
            return await NetworkTool.PostApiAsync<MusicDataResponse>(ApiUri.GetMusicData, ids);
        }
        #endregion

        #region Vtuber
        public async Task<VocalDataResponse> GetVocalListAsync(ListRequestUpper args)
        {
            return await NetworkTool.PostApiAsync<VocalDataResponse>(ApiUri.GetVtbsList, args);
        }
        #endregion

        #region 歌单
        public async Task<AlbumListResponse> GetAlbumListAsync(ListRequestUpper args)
        {
            return await NetworkTool.PostApiAsync<AlbumListResponse>(ApiUri.GetAlbumsList, args);
        }

        public async Task<AlbumDataResponse> GetAlbumDataAsync(string id)
        {
            return await NetworkTool.PostApiAsync<AlbumDataResponse>(ApiUri.GetAlbumsData, new GetAlbumDataRequeset { id = id });
        }
        #endregion

        #region Banner
        public async Task<BannerDataResponse> GetBannerListAsync(ListRequestUpper args)
        {
            return await NetworkTool.PostApiAsync<BannerDataResponse>(ApiUri.GetBannerData, args);
        }
        #endregion
    }

    public enum ResourcesType
    {
        Music = 0,
        Lyric = 1,
        CoverImg = 2,
    }
}
