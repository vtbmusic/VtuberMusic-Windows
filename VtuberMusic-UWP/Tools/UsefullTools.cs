using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Tools
{
    public class UsefullTools
    {
        public static DateTime ConvertUnixTimeStamp(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(time);
        }
    }
}
