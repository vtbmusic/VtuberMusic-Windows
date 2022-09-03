using Microsoft.UI.Xaml;
using VtuberMusic.App.Helper;
using VtuberMusic.App.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
    private readonly WindowBackdropHelper _backdropHelper;

    public MainWindow() {
        InitializeComponent();
        _backdropHelper = new WindowBackdropHelper(this);
        _backdropHelper.TrySetMicaBackdrop();

        Title = "VtuberMusic";
        ExtendsContentIntoTitleBar = true;

        App.MainWindow = this;
        App.RootFrame = RootFrame;
        DispatcherHelper.Init(DispatcherQueue);
        RootFrame.Navigate(typeof(LoginPage));
    }
}
