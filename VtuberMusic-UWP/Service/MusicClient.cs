using RestSharp;
using System;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Models.VtuberMusic.v1;

namespace VtuberMusic_UWP.Service {
    /// <summary>
    /// 音乐 api 客户端
    /// </summary>
    public class MusicClient {
        /// <summary>
        /// 账户服务
        /// </summary>
        public AccountService Account;
        private RestClient _restClient = new RestClient();

        public MusicClient() {
            this._restClient.UseSerializer<RestSharp.Serializers.NewtonsoftJson.JsonNetSerializer>();
            this.Account = new AccountService(this._restClient);
        }

        #region 歌曲
        /// <summary>
        /// 获取最新歌曲列表
        /// </summary>
        /// <param name="limit">数量</param>
        /// <returns>最新歌曲列表</returns>
        public async Task<ApiResponse<Music[]>> GetNewSong(int limit) {
            var request = new RestRequest(ApiUri.NewSong);
            request.AddParameter("limit", limit.ToString());

            var response = await this._restClient.ExecuteAsync<ApiResponse<Music[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取歌曲媒体 Url
        /// </summary>
        /// <param name="id">歌曲 ID</param>
        /// <returns>歌曲媒体 Url</returns>
        public async Task<ApiResponse<string>> GetSongMeduaUri(string id) {
            var request = new RestRequest(ApiUri.SongMedia + id);
            request.AddParameter("type", 1);

            var response = await this._restClient.ExecuteAsync<ApiResponse<string>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }
        #endregion

        /// <summary>
        /// 获取 Banner
        /// </summary>
        /// <param name="type">平台</param>
        /// <returns>Banner</returns>
        public async Task<ApiResponse<Banner[]>> GetBanner(string type = "pc") {
            var request = new RestRequest(ApiUri.Banner);
            request.AddParameter("type", type);

            var response = await this._restClient.ExecuteAsync<ApiResponse<Banner[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
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
        /// <returns>艺人列表</returns>
        public async Task<ApiResponse<Artist[]>> GetArtistList(char initial = ' ', string group = "", string area = "", int limit = 30, int offest = 0) {
            var request = new RestRequest(ApiUri.ArtistList);
            if (!char.IsWhiteSpace(initial)) request.AddParameter("initial", initial);
            if (!string.IsNullOrEmpty(group)) request.AddParameter("group", group);
            if (!string.IsNullOrEmpty(area)) request.AddParameter("area", area);

            request.AddParameter("limit", limit);
            request.AddParameter("offest", offest);

            var response = await this._restClient.ExecuteAsync<ApiResponse<Artist[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
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
        /// <returns>歌单列表</returns>
        public async Task<ApiResponse<Album[]>> GetPlayListList(int limit = 30, int offest = 0, string order = "time") {
            var request = new RestRequest(ApiUri.PlayListList);
            request.AddParameter("limit", limit);
            request.AddParameter("offest", offest);
            request.AddParameter("order", order);

            var response = await this._restClient.ExecuteAsync<ApiResponse<Album[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取歌单内歌曲列表
        /// </summary>
        /// <param name="id">歌单 id</param>
        /// <returns>歌单内歌曲列表</returns>
        public async Task<ApiResponse<AlbumSong>> GetPlayListSong(string id) {
            var request = new RestRequest(ApiUri.PlayListSongs);
            request.AddParameter("id", id);

            var response = await this._restClient.ExecuteAsync<ApiResponse<AlbumSong>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取歌手歌曲列表
        /// </summary>
        /// <param name="id">歌手 id</param>
        /// <param name="order">排序方法</param>
        /// <param name="limit">数量</param>
        /// <param name="offest">偏移量</param>
        /// <returns>歌手歌曲列表</returns>
        public async Task<ApiResponse<Music[]>> GetArtistSong(string id, string order = "time", int limit = 50, int offset = 0) {
            var request = new RestRequest(ApiUri.ArtistSongs);
            request.AddParameter("id", id);
            request.AddParameter("order", order);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = await this._restClient.ExecuteAsync<ApiResponse<Music[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 搜索音乐
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="limit">数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>音乐列表</returns>
        public async Task<ApiResponse<Music[]>> SearchMusic(string keyword, int limit = 100, int offset = 0) {
            var request = new RestRequest(ApiUri.Search);
            request.AddParameter("keyword", keyword);
            request.AddParameter("limit", limit);
            request.AddParameter("offest", offset);
            request.AddParameter("type", "song");

            var response = await this._restClient.ExecuteAsync<ApiResponse<Music[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);

        }

        /// <summary>
        /// 搜索歌手
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="limit">数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>歌手列表</returns>
        public async Task<ApiResponse<Artist[]>> SearchArtist(string keyword, int limit = 100, int offset = 0) {
            var request = new RestRequest(ApiUri.Search);
            request.AddParameter("keyword", keyword);
            request.AddParameter("limit", limit);
            request.AddParameter("offest", offset);
            request.AddParameter("type", "artist");

            var response = await this._restClient.ExecuteAsync<ApiResponse<Artist[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);

        }

        /// <summary>
        /// 搜索歌单
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="limit">数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns>歌单列表</returns>
        public async Task<ApiResponse<Album[]>> SearchPlaylist(string keyword, int limit = 100, int offset = 0) {
            var request = new RestRequest(ApiUri.Search);
            request.AddParameter("keyword", keyword);
            request.AddParameter("limit", limit);
            request.AddParameter("offest", offset);
            request.AddParameter("type", "playlist");

            var response = await this._restClient.ExecuteAsync<ApiResponse<Album[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取喜欢的音乐歌单信息
        /// </summary>
        /// <param name="uid">账户 id</param>
        /// <returns>音乐歌单信息</returns>
        public async Task<ApiResponse<AlbumSong>> GetLikeMusicList(string uid) {
            var request = new RestRequest(ApiUri.GetUserLikeMusic);
            request.AddParameter("uid", uid);

            var response = await this._restClient.ExecuteAsync<ApiResponse<AlbumSong>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取喜欢的音乐列表
        /// </summary>
        /// <param name="uid">账户 id</param>
        /// <returns>喜欢的音乐列表</returns>
        public async Task<ApiResponse<AlbumSong>> GetLikeMusicSong(string uid) {
            var request = new RestRequest(ApiUri.GetUserLikeMusic);
            request.AddParameter("uid", uid);
            request.AddParameter("type", "song");

            var response = await this._restClient.ExecuteAsync<ApiResponse<AlbumSong>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        #region v1
        public async Task<ApiResponseList<Comment[]>> GetMusicComment(string musicId, string parentId = "") {
            var request = new RestRequest(ApiUri.GetMusicCommentsV1, Method.POST);
            request.AddJsonBody(new {
                pageIndex = 1,
                pageRows = 1000,
                search = new {
                    musicId = musicId
                }
            });

            var response = await this._restClient.ExecuteAsync<ApiResponseList<Comment[]>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }
        #endregion
    }
}
