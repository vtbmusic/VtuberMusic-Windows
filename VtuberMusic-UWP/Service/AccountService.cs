using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;

namespace VtuberMusic_UWP.Service
{
    public class AccountService
    {
        public Account Account;
        public Profile Profile;
        public TokenPack Token;
        private RestClient _restClient;

        public AccountService(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<ApiResponse<LoginData>> Login(string userName, string password)
        {
            var request = new RestRequest(ApiUri.Login, Method.POST, DataFormat.Json);

            request.AddJsonBody(new
            {
                userName = userName,
                password = password
            });

            var response = await _restClient.ExecuteAsync<ApiResponse<LoginData>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success)
                {
                    Account = response.Data.Data.account;
                    Profile = response.Data.Data.profile;
                    Token = new TokenPack { access_token = response.Data.Data.access_token, refresh_token = response.Data.Data.refresh_token };

                    _restClient.Authenticator = new JwtAuthenticator(Token.access_token);
                    return response.Data;
                }

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse<AccountProfileData>> GetAccountInfo()
        {
            var request = new RestRequest(ApiUri.AccountInfo);

            var response = await _restClient.ExecuteAsync<ApiResponse<AccountProfileData>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success)
                {
                    Account = response.Data.Data.account;
                    Profile = response.Data.Data.profile;
                    return response.Data;
                }

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse<AccountSubCount>> GetAccountSubCouent()
        {
            var request = new RestRequest(ApiUri.SubCount);

            var response = await _restClient.ExecuteAsync<ApiResponse<AccountSubCount>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponseList<Album[]>> GetMyCreatePlayList()
        {
            var request = new RestRequest(ApiUri.MyCreatePlayList);

            var response = await _restClient.ExecuteAsync<ApiResponseList<Album[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponseList<Album[]>> GetMyFavouritePlayList()
        {
            var request = new RestRequest(ApiUri.MyFavouitePlayList);

            var response = await _restClient.ExecuteAsync<ApiResponseList<Album[]>>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse<AlbumSong>> GetLikeMusicList() => await App.Client.GetLikeMusicList(Account.id);

        public async Task<ApiResponse<AlbumSong>> GetLikeMusicSong() => await App.Client.GetLikeMusicSong(Account.id);

        public async Task<ApiResponse> LikeMusic(string id, bool like = true)
        {
            var request = new RestRequest(ApiUri.LikeMusic, Method.POST);
            request.AddParameter("id", id, ParameterType.QueryString);
            request.AddParameter("like", like.ToString().ToLower(), ParameterType.QueryString);

            var response = await _restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiResponse> CreateAlbum(string name, bool isPrivate = false)
        {
            var request = new RestRequest(ApiUri.CreateAlbum, Method.POST);
            request.AddParameter("name", name, ParameterType.QueryString);
            if (isPrivate) request.AddParameter("privacy", isPrivate, ParameterType.QueryString);

            var response = await _restClient.ExecuteAsync<ApiResponse>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success) return response.Data;

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }
    }

    public class TokenPack
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
