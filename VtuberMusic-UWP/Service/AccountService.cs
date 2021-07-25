using RestSharp;
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
        private RestClient _restClient = new RestClient();

        public AccountService()
        {
            _restClient.UseSerializer<RestSharp.Serializers.NewtonsoftJson.JsonNetSerializer>();
        }

        public async Task<ApiLoginResponse> Login(string userName, string password)
        {
            var request = new RestRequest(ApiUri.Login, Method.POST, DataFormat.Json);

            request.AddJsonBody(new
            {
                userName = userName,
                password = password
            });

            var response = await _restClient.ExecuteAsync<ApiLoginResponse>(request);

            if (response.IsSuccessful)
            {
                if (response.Data.Success)
                {
                    Account = response.Data.Data.account;
                    Profile = response.Data.Data.profile;
                    Token = new TokenPack { access_token = response.Data.access_token, refresh_token = response.Data.refresh_token };
                    return response.Data;
                }

                throw new Exception(response.Data.Msg);
            }

            if (response.ErrorException != null) throw response.ErrorException;
            throw new Exception(response.ErrorMessage);
        }

        public async Task<ApiAccountProfileResponse> GetAccountInfo()
        {
            var request = new RestRequest(ApiUri.AccountInfo);

            var response = await _restClient.ExecuteAsync<ApiAccountProfileResponse>(request);

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
            var request = new RestRequest(ApiUri.AccountInfo);

            var response = await _restClient.ExecuteAsync<ApiResponse<AccountSubCount>>(request);

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
