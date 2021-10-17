using System.Collections.ObjectModel;

namespace VtuberMusic_UWP.Models.DebugCommand {
    /// <summary>
    /// Debug 命令管理器
    /// </summary>
    public class DebugCommandManager {
        public ObservableCollection<IDebugCommand> CommandList = new ObservableCollection<IDebugCommand>();

        public DebugCommandManager() {
            this.CommandList.Add(new ForeGC());
            this.CommandList.Add(new ShowLog());
            this.CommandList.Add(new OpenProfile());

#if DEBUG // 只有 Debug 版启用的命令
            this.CommandList.Add(new ContentFrameGoForward());
            this.CommandList.Add(new RootFrameGoBack());
            this.CommandList.Add(new RootFrameGoForward());
#endif
        }
    }
}
