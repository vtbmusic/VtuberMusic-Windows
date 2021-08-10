using System.Numerics;
using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Setup : Page
    {
        public Setup()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            PopupShadow.Receivers.Add(BlurBackground);
            PopupWindow.Translation += new Vector3(0, 0, 42);
        }

        private void SetupPageFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var page = (ISetupStep)e.Content;

            IconOut.Completed += delegate
            {
                Icon.Content = page.Icon;
                IconIn.Begin();
            };

            IconOut.Begin();
        }
    }
}
