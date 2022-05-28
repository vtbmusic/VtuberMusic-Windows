using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/artist/detail")]
        Task<Models.ApiResponse<Artist>> GetArtistDetail(string id);

        [Get("/v2/artist/songs")]
        Task<Models.ApiResponse<Music[]>> GetArtistMusics(string id, ArtistMusicOrder order = ArtistMusicOrder.Hot, int limit = 50, int offset = 0);
    }
}
