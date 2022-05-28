﻿using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;

namespace VtuberMusic.App.PageArgs {
    public class PlaylistPageArg : PageArg {
        public Playlist Playlist { get; set; }
        public PlaylistType PlaylistType = PlaylistType.Playlist;
    }
}
