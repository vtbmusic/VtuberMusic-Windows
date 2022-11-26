using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v3/msg/notices")]
        Task<Models.ApiResponse<Notice[]>> GetSystemNotices(int limit = 50, int offset = 0);

        [Get("/v3/msg/comments")]
        Task<Models.ApiResponse<CommentNotice[]>> GetReplyNotices(int limit = 50, int offset = 0);

        [Get("/v3/msg/count")]
        Task<Models.ApiResponse<MessageCount>> GetNoticeCount();

        [Post("/v3/msg/read")]
        Task<Models.ApiResponse> ReadNotice(NoticeType type);
    }
}
