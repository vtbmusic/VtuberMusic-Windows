using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.Core.Messages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public class AuthorizationService : IAuthorizationService {
        public bool IsAuthorized { get; private set; }
        public bool IsLogin { get; private set; }
        public Profile Profile { get; private set; }
        public Account Account { get; private set; }
        public DateTimeOffset NextAuthorizationTime { get; private set; }

        public event EventHandler<bool> IsAuthorizedChanged;
        public event EventHandler<bool> IsLoginChanged;

        private readonly HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://api.aqua.chat") };

        private string accessToken;
        private string refreshToken;

        public AuthorizationService(string refreshToken = null) {
            if (refreshToken != null) IsLogin = true;
            this.refreshToken = refreshToken;
        }

        public async Task<LoginResponse> AuthorizationAsync() {
            if (!IsLogin) throw new Exception("未登录");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
            var result = await httpClient.PostAsync("/v2/user/login/refresh", new StringContent(""));

            var response = JsonConvert.DeserializeObject<ApiResponse<LoginResponse>>(await result.Content.ReadAsStringAsync());
            if (response.Success) {
                NextAuthorizationTime = DateTimeOffset.Now.AddDays(1);
                Profile = response.Data.profile;
                Account = response.Data.account;
                accessToken = response.Data.access_token;
                refreshToken = response.Data.refresh_token;
                IsAuthorized = true;

                WeakReferenceMessenger.Default.Send(new AuthorizationStateChangedMessage(response.Data));
                IsAuthorizedChanged?.Invoke(this, IsAuthorized);
                return response.Data;
            } else {
                IsAuthorizedChanged?.Invoke(this, IsAuthorized);
                throw new Exception(response.Msg);
            }
        }

        public async Task<string> GetTokenAsync() {
            if (!IsAuthorized | NextAuthorizationTime < DateTimeOffset.Now) await AuthorizationAsync();
            return accessToken;
        }

        public async Task<LoginResponse> LoginAsync(string userName, string password) {
            var result = httpClient.PostAsync("/v2/user/login", new StringContent(JsonConvert.SerializeObject(new {
                userName = userName,
                password = password
            }), Encoding.UTF8, "application/json"));

            var content = await result.Result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ApiResponse<LoginResponse>>(content);
            if (response.Success) {
                NextAuthorizationTime = DateTimeOffset.Now;
                Profile = response.Data.profile;
                Account = response.Data.account;
                accessToken = response.Data.access_token;
                refreshToken = response.Data.refresh_token;
                IsLogin = true;
                IsAuthorized = true;

                WeakReferenceMessenger.Default.Send(new AuthorizationStateChangedMessage(response.Data));
                IsAuthorizedChanged?.Invoke(this, IsAuthorized);
                IsLoginChanged?.Invoke(this, IsLogin);
            } else {
                throw new Exception(response.Msg);
            }

            return response.Data;
        }

        public string GetRefreshToken() {
            return refreshToken;
        }
    }
}
