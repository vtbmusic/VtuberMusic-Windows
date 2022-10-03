using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v3/comment/playlist?type=playlist")]
        Task<Models.ApiResponse<CommentResponse>> GetPlaylistComments(string id, int limit = 30, int offset = 0);

        [Get("/v3/comment/song?type=song")]
        Task<Models.ApiResponse<CommentResponse>> GetMusicComments(string id, int limit = 30, int offset = 0);

        [Get("/v3/comment/floor")]
        Task<Models.ApiResponse<CommentResponse>> GetFloorComments(string id, CommentContentType type, int limit, int offset);

        [Post("/v3/comment/add")]
        Task<ApiResponse> AddComment(string id, CommentContentType type, string content, string fid = null);

        [Post("/v3/comment/delete")]
        Task<ApiResponse> DeleteComment(string id, CommentContentType type);

        [Post("/v3/comment/like")]
        Task<ApiResponse> LikeComment(string id, string commentId, CommentContentType type, bool t = true);

        [Post("/v3/comment/unlike")]
        Task<ApiResponse> UnlikeComment(string id, string commentId, CommentContentType type, bool t = false);
    }
}
