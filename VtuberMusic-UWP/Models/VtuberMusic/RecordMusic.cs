using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    public class RecordMusic {
        public int playCount { get; set; }
        public string songId {  get; set; }
        public Music song { get; set; }
    }
}
