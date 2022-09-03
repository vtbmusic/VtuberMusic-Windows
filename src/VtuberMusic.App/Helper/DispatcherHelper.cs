using Microsoft.UI.Dispatching;

namespace VtuberMusic.App.Helper;
public class DispatcherHelper {
    public static DispatcherQueue Dispatcher { get; set; }

    public static void Init(DispatcherQueue dispatcher) => Dispatcher = dispatcher;

    public static void TryRun(DispatcherQueueHandler dispatchedHandler) => _ = Dispatcher.TryEnqueue(dispatchedHandler);
}
