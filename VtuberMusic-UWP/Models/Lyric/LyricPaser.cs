using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VtuberMusic_UWP.Models.Lyric {
    /// <summary>
    /// 歌词解析器
    /// </summary>
    public class LyricPaser {
        /// <summary>
        /// 解析歌词
        /// </summary>
        /// <param name="lyricString">歌词 string</param>
        /// <returns></returns>
        public static Lyric[] Parse(string lyricString) {
            var vrc = JsonConvert.DeserializeObject<Vrc>(lyricString);
            var lyrics = new List<Lyric>();
            var sourceLyric = parseLrc(vrc.origin.text);

            if (vrc.translate != null) {
                var translationLyric = parseLrc(vrc.translate.text);
                for (int i = 0; i < sourceLyric.Length || i < translationLyric.Length; i++) {
                    Debug.WriteLine(i);
                    TimeSpan time = TimeSpan.Zero;
                    string source = "";
                    string translation = "";

                    if (i < sourceLyric.Length) {
                        time = sourceLyric[i].Time;
                        source = sourceLyric[i].Text;
                    }

                    if (i < translationLyric.Length) {
                        time = translationLyric[i].Time;
                        translation = translationLyric[i].Text;
                    }

                    lyrics.Add(new Lyric { Time = time, Source = source, Translation = translation });
                }
            } else {
                for (int i = 0; i != sourceLyric.Length; i++) {
                    lyrics.Add(new Lyric { Time = sourceLyric[i].Time, Source = sourceLyric[i].Text, Translation = "" });
                }
            }

            return lyrics.ToArray();
        }

        private static LrcLyric[] parseLrc(string lrcString) {
            var offset = TimeSpan.Zero;
            var lyrics = new List<LrcLyric>();
            string[] contentLines = lrcString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string temp in contentLines) {
                if (temp.StartsWith("[offset:")) {
                    offset = TimeSpan.FromMilliseconds(int.Parse(SplitInfo(temp)));
                    break;
                }
            }

            Regex regex = new Regex(@"\[(.*?)\]([^[\]]*)\s*", RegexOptions.Compiled);
            var mc = regex.Matches(lrcString);

            foreach (Match temp in mc) {
                var time = TimeTransition(temp.Groups[1].Value, offset);
                var word = temp.Groups[2].Value.Replace("\n", "");

                lyrics.Add(new LrcLyric { Time = time, Text = word });
            }

            return lyrics.ToArray();
        }

        private static TimeSpan TimeTransition(string time, TimeSpan offset) {
            string[] splited = time.Split(":");
            switch (splited.Length) {
                // 00:00.00
                case 2:
                    string[] seconds = splited[1].Split(".");
                    return TimeSpan.FromMinutes(long.Parse(splited[0])) +
                        TimeSpan.FromSeconds(long.Parse(seconds[0])) +
                        TimeSpan.FromMilliseconds(long.Parse(seconds[1]));
                // 00.00 毒轴
                case 1:
                    string[] split = splited[0].Split(".");
                    return TimeSpan.FromSeconds(long.Parse(split[0])) +
                        TimeSpan.FromSeconds(float.Parse("0." + split[1]));
                default:
                    return TimeSpan.Zero;
            }
        }

        private static string SplitInfo(string line) {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }

        private class LrcLyric {
            public TimeSpan Time { get; set; }
            public string Text { get; set; }
        }
    }
}
