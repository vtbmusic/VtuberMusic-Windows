using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Components;

namespace VtuberMusic_UWP.Models.DebugCommand
{
    public class ShowLog : IDebugCommand
    {
        public string Title { get; } = "Show Log";
        public string Description { get; } = "显示日志";

        public void Do()
        {
            InfoBarPopup.Show("Todo", "咕咕咕");
        }
    }
}
