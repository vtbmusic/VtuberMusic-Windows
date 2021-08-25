using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.DebugCommand
{
    public class ContentFrameGoForward : IDebugCommand
    {
        public string Title { get; } = "ContentFrame GoForward";
        public string Description { get; } = "内容 Frame 下一页";

        public void Do() => App.ViewModel.MainPage.ContentFrame.GoForward();
    }

    public class RootFrameGoForward : IDebugCommand
    {
        public string Title { get; } = "RootFrame GoForward";
        public string Description { get; } = "Root Frame 下一页";

        public void Do() => App.RootFrame.GoForward();
    }

    public class RootFrameGoBack : IDebugCommand
    {
        public string Title { get; } = "RootFrame GoBack";
        public string Description { get; } = "Root Frame 上一页";

        public void Do() => App.RootFrame.GoBack();
    }
}
