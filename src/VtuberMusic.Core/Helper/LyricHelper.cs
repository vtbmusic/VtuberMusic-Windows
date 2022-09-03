using Opportunity.LrcParser;
using System.Collections.Generic;
using System.Threading.Tasks;
using VtuberMusic.Core.Models.Lyric;

namespace VtuberMusic.Core.Helper {
    public class LyricHelper {
        public static async Task<ParsedVrc> ParsedVrcAsync(Vrc vrc) {
            Lyrics<Line> translateLrc = null;
            var originLrc = Lyrics.Parse(vrc.origin.text).Lyrics;
            if (vrc.translated) translateLrc = Lyrics.Parse(vrc.translate.text).Lyrics;

            List<LyricWords> lyricsWords = new List<LyricWords>();
            for (int i = 0; i != originLrc.Lines.Count; i++) {
                var lyricWords = new LyricWords { Origin = originLrc.Lines[i] };
                if (translateLrc != null) {
                    lyricWords.Translate = translateLrc.Lines[i];
                }

                lyricsWords.Add(lyricWords);
            }

            return new ParsedVrc {
                Translated = translateLrc != null,
                Karaoke = vrc.karaoke,
                ScrollDisabled = vrc.scrollDisabled,
                Lyric = lyricsWords.ToArray(),
                TranslateLyric = translateLrc,
                OriginLyric = originLrc
            };
        }
    }
}
