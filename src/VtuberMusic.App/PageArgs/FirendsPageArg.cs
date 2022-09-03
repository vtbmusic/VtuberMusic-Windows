using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs;
public class FirendsPageArg : PageArg<Firends> {
    public Profile Profile { get; set; }
    public FirendsPageType Type { get; set; }
}
