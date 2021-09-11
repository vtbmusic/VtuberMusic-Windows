using System;

namespace VtuberMusic_UWP.Models.DebugCommand {
    /// <summary>
    /// 强制 GC Debug 命令
    /// </summary>
    public class ForeGC : IDebugCommand {
        public string Title { get; } = "Fore GC";
        public string Description { get; } = "强制执行 GC.Collect()";

        public void Do() => GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, false, true);
    }
}
