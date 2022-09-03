using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Helper;

namespace VtuberMusic.App.Controls;
public sealed partial class AppTitleBar : UserControl {
    public AppTitleBar() {
        InitializeComponent();
        TitleBarHelper.SetTitleBar(TitleBarArea);

        //TitleBarHelper.CoreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
    }

    private void CoreTitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args) => RightPaddingColumn.Width = new GridLength(TitleBarHelper.CoreTitleBar.SystemOverlayRightInset);
}