using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace VtuberMusic.App.Helper {
    public static class TitleBarHelper {
        public static ApplicationViewTitleBar AppTitleBar => AppView.TitleBar;
        public static ApplicationView AppView => ApplicationView.GetForCurrentView();

        public static CoreApplicationViewTitleBar CoreTitleBar => CoreAppView.TitleBar;
        public static CoreApplicationView CoreAppView => CoreApplication.GetCurrentView();

        public static string Title { get { return AppView.Title; } set { AppView.Title = value; } }

        public static void SetTitleBar(UIElement titleBar) => Window.Current.SetTitleBar(titleBar);
        public static void ChangeTheme(ApplicationTheme theme) {
            AppTitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);
            switch (theme) {
                case ApplicationTheme.Dark:
                    break;
                case ApplicationTheme.Light:
                    break;
            }
        }
    }
}