using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/artist/detail")]
        Task<Models.ApiResponse<Artist>> GetArtistDetail(string id);

        [Get("/v2/artist/songs")]
        Task<Models.ApiResponse<Music[]>> GetArtistMusics(string id, ArtistMusicOrder order = ArtistMusicOrder.Hot, int limit = 50, int offset = 0);

        [Get("/v2/artist/list")]
        Task<Models.ApiResponse<Artist[]>> GetArtists(string initial = null, string group = null, string area = null, int limit = 30, int offset = 0);

        [Get("/v2/search?type=artist")]
        Task<Models.ApiResponse<Artist[]>> SearchArtists(string keyword, int limit = 50, int offset = 0);
    }
}
