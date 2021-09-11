using VtuberMusic_UWP.Components;

namespace VtuberMusic_UWP.Models.DebugCommand {
    /// <summary>
    /// 显示日志 Debug 命令
    /// </summary>
    public class ShowLog : IDebugCommand {
        public string Title { get; } = "Show Log";
        public string Description { get; } = "显示日志";

        public void Do() {
            InfoBarPopup.Show("Todo", "咕咕咕");
        }
    }
}
