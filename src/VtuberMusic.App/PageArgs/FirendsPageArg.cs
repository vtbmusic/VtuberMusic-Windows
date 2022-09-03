using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs;
public class FriendsPageArg : PageArg<Friends> {
    public Profile Profile { get; set; }
    public FriendsPageType Type { get; set; }
}
