using System.Numerics;
using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 初始化页
    /// </summary>
    public sealed partial class Setup : Page {
        public Setup() {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.PopupShadow.Receivers.Add(this.BlurBackground);
            this.PopupWindow.Translation += new Vector3(0, 0, 42);
        }

        private void SetupPageFrame_Navigated(object sender, NavigationEventArgs e) {
            var page = (ISetupStep)e.Content;

            this.IconOut.Completed += delegate {
                this.Icon.Content = page.Icon;
                this.IconIn.Begin();
            };

            this.IconOut.Begin();
        }
    }
}
