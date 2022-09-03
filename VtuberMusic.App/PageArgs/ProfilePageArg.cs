using VtuberMusic.App.Pages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs {
    public class ProfilePageArg : PageArg<ProfilePage> {
        public Profile Profile { get; set; }
    }
}
