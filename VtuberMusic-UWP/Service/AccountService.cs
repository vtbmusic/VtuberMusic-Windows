using Microsoft.AppCenter.Analytics;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Tools;

namespace VtuberMusic_UWP.Service {
    /// <summary>
    /// 账户服务
    /// </summary>
    public class AccountService : INotifyPropertyChanged {
        private Account _account { get; set; }
        /// <summary>
        /// 当前登录账户的账户信息
        /// </summary>
        public Account Account {
            get { return this._account; }
            set {
                this._account = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.Logged));
            }
        }

        private Profile _profile { get; set; }
        /// <summary>
        /// 当前登录账户的账户资料
        /// </summary>
        public Profile Profile {
            get { return this._profile; }
            set {
                this._profile = value;
                this.OnPropertyChanged();
            }
        }

        private TokenPack _tokenPack { get; set; }
        /// <summary>
        /// Token 包
        /// </summary>
        public TokenPack Token {
            get { return this._tokenPack; }
            set {
                this._tokenPack = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool Logged { get { return this.Account != null ? true : false; } }

        public event EventHandler<AccountProfileData> LoginStatueChanged;

        private RestClient _restClient;

        public AccountService(RestClient restClient) {
            this._restClient = restClient;
        }

        #region ViewModel
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>Access Token / Refresh Token</returns>
        public async Task<ApiResponse<LoginData>> Login(string userName, string password) {
            var request = new RestRequest(ApiUri.Login, Method.POST, DataFormat.Json);

            request.AddJsonBody(new {
                userName = userName,
                password = password
            });

            var response = await this._restClient.ExecuteAsync<ApiResponse<LoginData>>(request);

            if (response.IsSuccessful) {
                if (response.Data.Success) {
                    this.Account = response.Data.Data.account;
                    this.Profile = response.Data.Data.profile;
                    this.Token = new TokenPack { access_token = response.Data.Data.access_token, refresh_token = response.Data.Data.refresh_token };

                    this._restClient.Authenticator = new JwtAuthenticator(this.Token.access_token);
                    this.LoginStatueChanged?.Invoke(this, new AccountProfileData { account = this.Account, profile = this.Profile });
                    return response.Data;
                }

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <returns>返回账户信息</returns>
        public async Task<ApiResponse<AccountProfileData>> GetAccountInfo() {
            var request = new RestRequest(ApiUri.AccountInfo);

            var response = await this._restClient.ExecuteAsync<ApiResponse<AccountProfileData>>(request);

            if (response.IsSuccessful) {
                if (response.Data.Success) {
                    this.Account = response.Data.Data.account;
                    this.Profile = response.Data.Data.profile;
                    return response.Data;
                }

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取账户 4 个项目的数值
        /// </summary>
        /// <returns>主推数量 / 喜欢歌曲数量 / 创建的歌单数量 / 收藏的歌单数量 </returns>
        public async Task<ApiResponse<AccountSubCount>> GetAccountSubCouent() {
            var request = new RestRequest(ApiUri.SubCount);

            var response = await this._restClient.ExecuteAsync<ApiResponse<AccountSubCount>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取该账户创建的歌单列表
        /// </summary>
        /// <returns>该账户创建的歌单列表</returns>
        public async Task<ApiResponseList<Album[]>> GetMyCreatePlayList() => await App.Client.GetMyCreatePlayList(this.Profile.userId);

        /// <summary>
        /// 获取该账户收藏的歌单列表
        /// </summary>
        /// <returns>该账户收藏的歌单列表</returns>
        public async Task<ApiResponseList<Album[]>> GetMyFavouritePlayList() => await App.Client.GetMyFavouritePlayList(this.Profile.userId);

        /// <summary>
        /// 获取 "我喜欢的音乐" 歌单信息
        /// </summary>
        /// <returns>"我喜欢的音乐" 歌单信息</returns>
        public async Task<ApiResponse<AlbumSong>> GetLikeMusicList() => await App.Client.GetLikeMusicList(this.Account.id);

        /// <summary>
        /// 获取喜欢的音乐列表
        /// </summary>
        /// <returns>喜欢的音乐列表</returns>
        public async Task<ApiResponse<AlbumSong>> GetLikeMusicSong() => await App.Client.GetLikeMusicSong(this.Account.id);

        /// <summary>
        /// (不) 喜欢音乐
        /// </summary>
        /// <param name="id">音乐 id</param>
        /// <param name="like">是否喜欢</param>
        /// <returns>操作是否成功</returns>
        public async Task<ApiResponse> LikeMusic(string id, bool like = true) {
            Analytics.TrackEvent("喜欢音乐", new Dictionary<string, string>() {
                { "music_id", id },
                { "like", like.ToString() }
            });

            var request = new RestRequest(ApiUri.LikeMusic, Method.POST);
            request.AddParameter("id", id, ParameterType.QueryString);
            request.AddParameter("like", like.ToString().ToLower(), ParameterType.QueryString);

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 创建歌单
        /// </summary>
        /// <param name="name">歌单名称</param>
        /// <param name="isPrivate">是否为隐私歌单</param>
        /// <returns>操作是否成功</returns>
        public async Task<ApiResponse> CreateAlbum(string name, bool isPrivate = false) {
            var request = new RestRequest(ApiUri.CreateAlbum, Method.POST);
            request.AddParameter("name", name, ParameterType.QueryString);
            if (isPrivate) request.AddParameter("privacy", isPrivate, ParameterType.QueryString);

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 添加音乐到歌单
        /// </summary>
        /// <param name="pid">歌单 id</param>
        /// <param name="type">增 / 删</param>
        /// <param name="musicIds">音乐 id</param>
        /// <returns>操作是否成功</returns>
        public async Task<ApiResponse> TrackMusic(string pid, TrackType type, string[] musicIds) {
            Analytics.TrackEvent("添加音乐到歌单");

            var request = new RestRequest(ApiUri.TrackMusic, Method.POST);
            request.AddParameter("pid", pid, ParameterType.QueryString);
            request.AddParameter("type", type.ToString(), ParameterType.QueryString);
            request.AddParameter("tracks", UsefullTools.ConvertStringArrayToString(musicIds), ParameterType.QueryString);

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 编辑歌单信息
        /// </summary>
        /// <param name="id">歌单 id</param>
        /// <param name="name">歌单名称</param>
        /// <param name="desc">?</param>
        /// <param name="tags">标签</param>
        /// <returns>操作是否成功</returns>
        public async Task<ApiResponse> EditAlbumInfo(string id, string name, string desc, string[] tags = null) {
            Analytics.TrackEvent("编辑歌单信息");

            tags = new string[] { "tag" };
            var request = new RestRequest(ApiUri.EditAlbum, Method.POST);
            request.AddParameter("id", id, ParameterType.QueryString);
            request.AddParameter("name", name, ParameterType.QueryString);
            request.AddParameter("desc", desc, ParameterType.QueryString);
            request.AddParameter("tags", UsefullTools.ConvertStringArrayToString(tags), ParameterType.QueryString);

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse> SubscribeAlbum(string id, bool like) {
            Analytics.TrackEvent("收藏歌单", new Dictionary<string, string>() {
                { "Album_Id", id },
                { "Like", like.ToString() }
            });

            var request = new RestRequest(ApiUri.SubscribeAlbum, Method.POST);
            request.AddQueryParameter("id", id);
            request.AddQueryParameter("like", like.ToString());

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse> SendCode(string email) {
            var request = new RestRequest(ApiUri.GetCaptchaCode, Method.POST);
            request.AddQueryParameter("email", email);

            var response = await this._restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse<AccountProfileData>> Register(string email, string password, string nickname, string code) {
            var request = new RestRequest(ApiUri.Register, Method.POST);
            request.AddQueryParameter("email", email);
            request.AddQueryParameter("password", password);
            request.AddQueryParameter("nickname", nickname);
            request.AddQueryParameter("code", code);

            var response = await this._restClient.ExecuteAsync<ApiResponse<AccountProfileData>>(request);

            if (response.IsSuccessful) {
                return response.Data.Success ? response.Data : throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        /// <summary>
        /// 获取历史播放音乐
        /// </summary>
        /// <param name="type">type=0 时返回 allData，type=1 时只返回最近一周数据</param>
        /// <returns>播放历史记录</returns>
        public async Task<ApiResponse<RecordMusic[]>> GetRecord(int type = 1) => await App.Client.GetMusicRecordList(this.Account.id, type);
    }

    /// <summary>
    /// Token 包，存储 access_token / refresh_token
    /// </summary>
    public class TokenPack {
        /// <summary>
        /// 访问 Token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 刷新 Token
        /// </summary>
        public string refresh_token { get; set; }
    }

    /// <summary>
    /// 歌单音乐增删操作
    /// </summary>
    public enum TrackType {
        /// <summary>
        /// 添加
        /// </summary>
        add,
        /// <summary>
        /// 删除
        /// </summary>
        del
    }
}
