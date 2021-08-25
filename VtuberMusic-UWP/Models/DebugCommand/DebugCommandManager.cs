using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.DebugCommand
{
    public class DebugCommandManager
    {
        public ObservableCollection<IDebugCommand> CommandList = new ObservableCollection<IDebugCommand>();

        public DebugCommandManager()
        {
            CommandList.Add(new ForeGC());
            CommandList.Add(new ShowLog());

#if DEBUG // 只有 Debug 版启用的命令
            CommandList.Add(new ContentFrameGoForward());
            CommandList.Add(new RootFrameGoBack());
            CommandList.Add(new RootFrameGoForward());
#endif
        }
    }
}
