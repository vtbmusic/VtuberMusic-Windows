﻿using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/user/detail/{uid}")]
        Task<Models.ApiResponse<ProfileResponse>> GetProfile([AliasAs("uid")] string uid);
    }
}
