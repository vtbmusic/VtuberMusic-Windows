using Opportunity.LrcParser;

namespace VtuberMusic.Core.Models.Lyric {

    public class Vrc {
        public bool karaoke { get; set; }
        public bool scrollDisabled { get; set; }
        public bool translated { get; set; }
        public VrcLyric origin { get; set; }
        public VrcLyric translate { get; set; }
    }

    public class VrcLyric {
        public int version { get; set; }
        public string text { get; set; }
    }

    public class ParsedVrc {
        public bool Karaoke { get; set; }
        public bool ScrollDisabled { get; set; }
        public bool Translated { get; set; }
        public LyricWords[] Lyric { get; set; }
        public Lyrics<Line> TranslateLyric { get; set; }
        public Lyrics<Line> OriginLyric { get; set; }
    }
}
