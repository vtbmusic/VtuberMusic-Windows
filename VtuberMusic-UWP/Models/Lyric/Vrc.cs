namespace VtuberMusic_UWP.Models.Lyric {
    /// <summary>
    /// Vrc Object
    /// </summary>
    public class Vrc {
        /// <summary>
        /// KTV 特性
        /// </summary>
        public bool karaoke { get; set; }
        /// <summary>
        /// 是否关闭滚动
        /// </summary>
        public bool scrollDisabled { get; set; }
        /// <summary>
        /// 是否翻译
        /// </summary>
        public bool translated { get; set; }
        /// <summary>
        /// 源文 vrc 歌词
        /// </summary>
        public VrcLyric origin { get; set; }
        /// <summary>
        /// 翻译 vrc 歌词
        /// </summary>
        public VrcLyric translate { get; set; }
    }

    /// <summary>
    /// vrc 歌词
    /// </summary>
    public class VrcLyric {
        /// <summary>
        /// 版本
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string text { get; set; }
    }
}