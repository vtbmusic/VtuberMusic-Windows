using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using WinRT;

namespace VtuberMusic.App.Helper;
public class WindowBackdropHelper {
    private readonly Window BackdropWindow;
    private readonly WindowsSystemDispatcherQueueHelper _wsdqHelper;
    private DesktopAcrylicController _acrylicController;
    private MicaController _micaController;

    public SystemBackdropConfiguration ConfigurationSource = new();

    public WindowBackdropHelper(Window window) {
        BackdropWindow = window;

        _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
        BackdropWindow.Activated += Window_Activated;
        BackdropWindow.Closed += Window_Closed;
    }

    public void AutoSetBackdrop() {
        if (!TrySetMicaBackdrop()) {
            _ = TrySetAcrylicBackdrop();
        }
    }

    public bool TrySetAcrylicBackdrop() {
        if (DesktopAcrylicController.IsSupported()) {
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            ((FrameworkElement)BackdropWindow.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            ConfigurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            _acrylicController = new DesktopAcrylicController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            _ = _acrylicController.AddSystemBackdropTarget(BackdropWindow.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _acrylicController.SetSystemBackdropConfiguration(ConfigurationSource);
            return true; // succeeded
        }

        return false; // Acrylic is not supported on this system
    }

    public bool TrySetMicaBackdrop() {
        if (MicaController.IsSupported()) {
            _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            ((FrameworkElement)BackdropWindow.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            ConfigurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            _micaController = new MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            _ = _micaController.AddSystemBackdropTarget(BackdropWindow.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            _micaController.SetSystemBackdropConfiguration(ConfigurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on this system
    }

    private void SetConfigurationSourceTheme() {
        switch (((FrameworkElement)BackdropWindow.Content).ActualTheme) {
            case ElementTheme.Dark: ConfigurationSource.Theme = SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: ConfigurationSource.Theme = SystemBackdropTheme.Light; break;
            case ElementTheme.Default: ConfigurationSource.Theme = SystemBackdropTheme.Default; break;
        }
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args) {
        if (ConfigurationSource != null) {
            SetConfigurationSourceTheme();
        }
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args) => ConfigurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;

    private void Window_Closed(object sender, WindowEventArgs args) {
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use this closed window.
        if (_micaController != null) {
            _micaController.Dispose();
            _micaController = null;
        }

        if (_acrylicController != null) {
            _acrylicController.Dispose();
            _acrylicController = null;
        }

        BackdropWindow.Activated -= Window_Activated;
        ConfigurationSource = null;
    }
}
