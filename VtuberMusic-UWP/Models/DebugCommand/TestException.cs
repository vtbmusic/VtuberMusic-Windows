using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.DebugCommand {
    public class TestException : IDebugCommand {
        public string Title { get; } = "TestException";

        public string Description { get; } = "测试崩溃";

        public void Do() {
            Crashes.TrackError(new Exception("Text Exception"));
            throw new Exception("Text Exception");
        }
    }
}
