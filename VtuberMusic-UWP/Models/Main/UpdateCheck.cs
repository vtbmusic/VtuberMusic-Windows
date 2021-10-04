using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.Main {
    /// <summary>
    /// 检查更新信息
    /// </summary>
    public class UpdateCheck {
        public string version { get; set; }
        public string commit { get; set; }
        public string log { get; set; }
        public string url { get; set; }
    }
}
