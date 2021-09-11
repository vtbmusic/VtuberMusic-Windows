using System;
using VtuberMusic_UWP.Models.VtuberMusic;

namespace VtuberMusic_UWP.Tools {
    /// <summary>
    /// "Usefull" Tools
    /// </summary>
    public class UsefullTools {
        /// <summary>
        /// 转换 Unix TimeStamp 到 DateTime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixTimeStamp(long time) {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }

        /// <summary>
        /// 获取多个歌手 Object 的歌手名称字符串
        /// </summary>
        /// <param name="artists"></param>
        /// <returns></returns>
        public static string GetArtistsString(Artist[] artists) {
            string artist = "";
            foreach (var temp in artists) {
                artist += temp.name.origin + " ";
            };

            return artist;
        }

        /// <summary>
        /// 转换 String [] 到 String
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string ConvertStringArrayToString(string[] strings) {
            var result = "";
            for (int i = 0; i != strings.Length; i++) {
                result += strings[i];
                if (i != strings.Length - 1) result += ",";
            }

            return result;
        }
    }
}
