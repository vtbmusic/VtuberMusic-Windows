using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.Lyric
{
    public class Vrc
    {
        public bool karaoke { get; set; }
        public bool scrollDisabled { get; set; }
        public bool translated { get; set; }
        public VrcLyric origin { get; set; }
        public VrcLyric translate { get; set; }
    }

    public class VrcLyric
    {
        public int version { get; set; }
        public string text { get; set; }
    }
}