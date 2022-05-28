using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace VtuberMusic.App.Helper {
    public class DispatcherHelper {
        public static CoreDispatcher Dispatcher { get; set; }

        public static void Init() {
            Dispatcher = Window.Current.Dispatcher;
        }

        public async static void TryRun(DispatchedHandler dispatchedHandler) {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, dispatchedHandler);
        }
    }
}
