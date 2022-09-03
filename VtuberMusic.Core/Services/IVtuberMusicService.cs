using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    [Headers("Authorization: Bearer")]
    public partial interface IVtuberMusicService {
        [Get("/v2/banner")]
        Task<Models.ApiResponse<Banner[]>> GetBanner();
    }
}
