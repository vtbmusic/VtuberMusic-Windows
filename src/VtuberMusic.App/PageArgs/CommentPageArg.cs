using VtuberMusic.App.Pages;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs;
public class CommentPageArg : PageArg<CommentPage> {
    public Music Music { get; set; }
    public Playlist Playlist { get; set; }
    public CommentContentType Type { get; set; }
}
