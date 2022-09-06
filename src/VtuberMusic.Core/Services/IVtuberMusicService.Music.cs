using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Models.Lyric;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/song/news")]
        Task<Models.ApiResponse<Music[]>> GetNewMusic(int limit = 12);

        [Get("/v3/personalized/song")]
        Task<Models.ApiResponse<Music[]>> GetPersonalizedMusic(int limit = 12);

        [Get("/v2/search?type=song")]
        Task<Models.ApiResponse<Music[]>> SearchMusic(string keyword, int limit = 50, int offset = 0);

        [Get("/lyric")]
        Task<Vrc> GetLyric(string id);

        [Get("/v2/song/url/media/{id}")]
        Task<Models.ApiResponse<string>> GetSongUrl(string id, int type = 1);
    }
}
