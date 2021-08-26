using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.VtuberMusic;

namespace VtuberMusic_UWP.Tools
{
    public class UsefullTools
    {
        public static DateTime ConvertUnixTimeStamp(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }

        public static string GetArtistsString(Artist[] artists)
        {
            string artist = "";
            foreach (var temp in artists)
            {
                artist += temp.name.origin + " ";
            };

            return artist;
        }

        public static string ConvertStringArrayToString(string[] strings)
        {
            var result = "";
            for (int i = 0; i != strings.Length; i++)
            {
                result += strings[i];
                if (i != strings.Length - 1) result += ",";
            }

            return result;
        }
    }
}
