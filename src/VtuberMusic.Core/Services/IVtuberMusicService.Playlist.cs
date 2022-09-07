using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
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

        [Post("/v2/playlist/tracks")]
        Task<Models.ApiResponse> TrackMusic(string pid, TrackType type, [Query(CollectionFormat.Csv)] string[] tracks);

        [Post("/v2/playlist/create")]
        Task<Models.ApiResponse> CreatePlaylist(string name, string isPrivacy = null);

        [Post("/v2/playlist/delete")]
        Task<Models.ApiResponse> DeletePlaylist(string id);

        [Post("/v2/playlist/subscribe")]
        Task<Models.ApiResponse> SubscribePlaylist(string id, bool like);

        [Post("/v2/song/updateorder/order/update")]
        Task<Models.ApiResponse> ReorderPlaylistMusic(string pid, [Query(CollectionFormat.Csv)] string[] ids);
    }
}
