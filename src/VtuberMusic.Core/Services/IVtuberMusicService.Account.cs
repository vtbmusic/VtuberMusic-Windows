﻿using Refit;
using System.Threading.Tasks;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public partial interface IVtuberMusicService {
        [Get("/v2/user/detail/{uid}")]
        Task<Models.ApiResponse<ProfileResponse>> GetProfile([AliasAs("uid")] string uid);

        [Get("/v2/user/follows")]
        Task<Models.ApiResponse<Profile[]>> GetFollows(string uid, int limit = 30, int offset = 0);

        [Get("/v2/user/fans")]
        Task<Models.ApiResponse<Profile[]>> GetFans(string uid, int limit = 30, int offset = 0);

        [Post("/v2/user/follow")]
        Task<Models.ApiResponse> Follow(string id, string t = null);

        [Get("/v2/search?type=user")]
        Task<Models.ApiResponse<Profile[]>> SearchUser(string keyword, int limit = 50, int offset = 0);

        [Post("/v2/user/update")]
        Task<Models.ApiResponse> UpdateProfile(ProfileGender gender, long birthday, string nickname, string signature);
    }
}
