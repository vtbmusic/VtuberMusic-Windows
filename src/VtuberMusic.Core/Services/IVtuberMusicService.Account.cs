using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/user/detail/{uid}")]
        Task<Models.ApiResponse<ProfileResponse>> GetProfile([AliasAs("uid")] string uid);

        [Get("/v2/user/follows")]
        Task<Models.ApiResponse<Profile[]>> GetFollows(string uid, int limit = 30, int offset = 0);

        [Get("/v2/user/fans")]
        Task<Models.ApiResponse<Profile[]>> GetFans(string uid, int limit = 30, int offset = 0);
    }
}
