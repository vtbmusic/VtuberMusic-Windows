using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.DebugCommand
{
    public class ForeGC : IDebugCommand
    {
        public string Title { get; } = "Fore GC";
        public string Description { get; } = "强制执行 GC.Collect()";

        public void Do() => GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, false, true);
    }
}
