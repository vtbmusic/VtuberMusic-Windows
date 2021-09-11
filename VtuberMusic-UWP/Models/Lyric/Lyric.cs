using System;

namespace VtuberMusic_UWP.Models.Lyric {
    /// <summary>
    /// 歌词
    /// </summary>
    public class Lyric {
        /// <summary>
        /// 时间轴
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// 源文
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 翻译
        /// </summary>
        public string Translation { get; set; }
    }
}
