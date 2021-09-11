using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages.SetupPage {
    /// <summary>
    /// 初始化语言选择页面
    /// </summary>
    public sealed partial class Welcome : Page, ISetupStep {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE164" };

        public Welcome() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(Account), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
