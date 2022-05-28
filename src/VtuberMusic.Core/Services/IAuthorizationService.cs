using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Services {
    public interface IAuthorizationService {
        bool IsAuthorized { get; }
        bool IsLogin { get; }
        Profile Profile { get; }
        Account Account { get; }

        event EventHandler<bool> IsAuthorizedChanged;
        event EventHandler<bool> IsLoginChanged;

        Task<LoginResponse> LoginAsync(string userName, string password);
        Task<LoginResponse> AuthorizationAsync();
        Task<string> GetTokenAsync();
        string GetRefreshToken();
    }
}