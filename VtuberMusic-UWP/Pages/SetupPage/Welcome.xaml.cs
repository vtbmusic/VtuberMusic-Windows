using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages.SetupPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Welcome : Page, ISetupStep
    {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE164" };

        public Welcome()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Account), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
