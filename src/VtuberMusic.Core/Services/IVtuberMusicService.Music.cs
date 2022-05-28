﻿using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/song/news")]
        Task<Models.ApiResponse<Music[]>> GetNewMusic(int limit = 12);

        [Get("/v3/personalized/song")]
        Task<Models.ApiResponse<Music[]>> GetPersonalizedMusic(int limit = 12);

        [Get("/v2/search?type=song")]
        Task<Models.ApiResponse<Music[]>> SearchMusic(string keyword, int limit = 50, int offset = 0);
    }
}
