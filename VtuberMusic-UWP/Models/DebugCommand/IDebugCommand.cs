namespace VtuberMusic_UWP.Models.DebugCommand {
    /// <summary>
    /// Debug 命令
    /// </summary>
    public interface IDebugCommand {
        string Title { get; }
        string Description { get; }

        void Do();
    }
}
