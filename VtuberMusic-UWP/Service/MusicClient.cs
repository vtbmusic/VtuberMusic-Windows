using RestSharp;
using System;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;

namespace VtuberMusic_UWP.Service
{
    public class MusicClient
    {
        public AccountService Account = new AccountService();
        private RestClient _restClient = new RestClient();

        public MusicClient()
        {
            _restClient.UseSerializer<RestSharp.Serializers.NewtonsoftJson.JsonNetSerializer>();
        }

        #region 歌曲
        /// <summary>
        /// 获取最新歌曲
        /// </summary>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        public async Task<ApiResponse<Music[]>> GetNewSong(int limit)
        {
            var request = new RestRequest(ApiUri.NewSong);
            request.AddParameter("limit", limit.ToString());

            var response = await _restClient.ExecuteAsync<ApiResponse<Music[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取歌曲媒体 Url
        /// </summary>
        /// <param name="id">歌曲 ID</param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> GetSongMeduaUri(string id)
        {
            var request = new RestRequest(ApiUri.SongMedia + id);
            request.AddParameter("type", 1);

            var response = await _restClient.ExecuteAsync<ApiResponse<string>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }
        #endregion

        /// <summary>
        /// 获取 Banner
        /// </summary>
        /// <param name="type">平台</param>
        /// <returns></returns>
        public async Task<ApiResponse<Banner[]>> GetBanner(string type = "pc")
        {
            var request = new RestRequest(ApiUri.Banner);
            request.AddParameter("type", type);

            var response = await _restClient.ExecuteAsync<ApiResponse<Banner[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取艺人列表
        /// </summary>
        /// <param name="initial">开头字母</param>
        /// <param name="group">所属组织名称</param>
        /// <param name="area">主要语言 cn en jp kr id</param>
        /// <param name="limit">数量</param>
        /// <param name="offest">偏移量</param>
        /// <returns></returns>
        public async Task<ApiResponse<Artist[]>> GetArtistList(char initial = ' ', string group = "", string area = "", int limit = 30, int offest = 0)
        {
            var request = new RestRequest(ApiUri.ArtistList);
            if (!char.IsWhiteSpace(initial)) request.AddParameter("initial", initial);
            if (!string.IsNullOrEmpty(group)) request.AddParameter("group", group);
            if (!string.IsNullOrEmpty(area)) request.AddParameter("area", area);

            request.AddParameter("limit", limit);
            request.AddParameter("offest", offest);

            var response = await _restClient.ExecuteAsync<ApiResponse<Artist[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取歌单列表
        /// </summary>
        /// <param name="limit">数量</param>
        /// <param name="offest">偏移量</param>
        /// <param name="order">排序方法 hot time</param>
        /// <returns></returns>
        public async Task<ApiResponse<Album[]>> GetPlayListList(int limit = 30, int offest = 0, string order = "time")
        {
            var request = new RestRequest(ApiUri.PlayListList);
            request.AddParameter("limit", limit);
            request.AddParameter("offest", offest);
            request.AddParameter("order", order);

            var response = await _restClient.ExecuteAsync<ApiResponse<Album[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse<AlbumSong>> GetPlayListSong(string id)
        {
            var request = new RestRequest(ApiUri.PlayListSongs);
            request.AddParameter("id", id);

            var response = await _restClient.ExecuteAsync<ApiResponse<AlbumSong>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;
                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }
    }
}
