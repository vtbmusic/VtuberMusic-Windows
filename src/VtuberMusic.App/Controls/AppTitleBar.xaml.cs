using VtuberMusic.App.Helper;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic.App.Controls {
    public sealed partial class AppTitleBar : UserControl {
        public AppTitleBar() {
            this.InitializeComponent();
            TitleBarHelper.SetTitleBar(TitleBarArea);

            TitleBarHelper.CoreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
        }

        private void CoreTitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args) {
            RightPaddingColumn.Width = new GridLength(TitleBarHelper.CoreTitleBar.SystemOverlayRightInset);
        }
    }
}