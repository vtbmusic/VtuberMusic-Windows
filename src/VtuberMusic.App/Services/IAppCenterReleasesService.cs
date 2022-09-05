using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.AppCore.Models;

namespace VtuberMusic.App.Services;
public interface IAppCenterReleasesService {
    [Get("/api/v0.1/apps/vtubermusic-misaka-l/vtubermusic-uwp/distribution_groups/users/public_releases?scope=tester")]
    Task<AppCenterReleasesItem[]> GetReleasesAsync();

    [Get("/api/v0.1/apps/vtubermusic-misaka-l/vtubermusic-uwp/distribution_groups/users/releases/{id}?is_install_page=true")]
    Task<AppCenterReleases> GetReleaseAsync([AliasAs("id")] int id);
}
