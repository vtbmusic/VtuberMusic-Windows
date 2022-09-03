using VtuberMusic.App.Pages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs;
public class ArtistPageArg : PageArg<ArtistPage> {
    public Artist Artist { get; set; }
}
