using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/playlist/list")]
        Task<Models.ApiResponse<Playlist[]>> GetPlaylists(int limit = 30, int offset = 0, string order = "hot");

        [Get("/v2/playlist/song")]
        Task<Models.ApiResponse<PlaylistMusicsResponse>> GetPlaylistMusics(string id);

        [Get("/v3/personalized/day/song")]
        Task<Models.ApiResponse<PlaylistMusicsResponse>> GetDailyPersonalizedMusic(int limit = 30);

        [Get("/v2/search?type=playlist")]
        Task<Models.ApiResponse<Playlist[]>> SearchPlaylist(string keyword, int limit = 50, int offset = 0);

        [Get("/v2/playlist/createlist")]
        Task<Models.ApiResponse<Playlist[]>> GetCreatePlaylist(string uid = "");

        [Get("/v2/playlist/sublist")]
        Task<Models.ApiResponse<Playlist[]>> GetSubPlaylist(string uid = "");

        [Get("/v2/playlist/likelist")]
        Task<Models.ApiResponse<PlaylistMusicsResponse>> GetFavouriteMusicsPlaylist(string type = "", string uid = "");
    }
}
