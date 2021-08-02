using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.Lyric
{
    public class Lyric
    {
        public TimeSpan Time { get; set; }
        public string Source { get; set; }
        public string Translation { get; set; }
    }
}
