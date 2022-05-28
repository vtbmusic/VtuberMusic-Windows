using System.Collections.Generic;
using System.Linq;
using VtuberMusic.Core.Models;

namespace VtuberMusic.AppCore.Helper {
    public class MusicHelepr {
        public static string GetArtistString(IEnumerable<Artist> artists) {
            switch (artists.Count()) {
                case 1:
                    return artists.ElementAt(0).name.origin;
                case 2:
                    return $"{ artists.ElementAt(0).name.origin } & { artists.ElementAt(1).name.origin }";
                default:
                    string text = "";
                    for (int i = 0; i != artists.Count() - 2; i++)
                        text += $"{ artists.ElementAt(i).name.origin }, ";
                    text += $"{ artists.ElementAt(artists.Count() - 2).name.origin } & { artists.ElementAt(artists.Count() - 1).name.origin }";
                    return text;
            }
        }
    }
}
