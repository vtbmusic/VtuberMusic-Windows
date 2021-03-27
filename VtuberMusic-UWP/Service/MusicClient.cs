using RestSharp;
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
        private RestClient _restClient = new RestClient();

        public MusicClient()
        {
            _restClient.UseSerializer<RestSharp.Serializers.NewtonsoftJson.JsonNetSerializer>();
        }

        #region 歌曲
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
    }
}
