using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.Main;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages.SetupPage {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RegisterNickname : Page, ISetupStep {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE902" };
        private SetupRegisterData data;

        public RegisterNickname() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            data = e.Parameter as SetupRegisterData;
        }

        private void Nickname_TextChanged(object sender, TextChangedEventArgs e) {
            this.Next.IsEnabled = Nickname.Text != "";
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            this.Frame.GoBack();
        }

        private async void Next_Click(object sender, RoutedEventArgs e) {
            data.Nickname = Nickname.Text;
            this.IsEnabled = false;

            try {
                await App.Client.Account.SendCode(data.Username);
                this.Frame.Navigate(typeof(RegisterFinish),
                    data,
                    new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            } catch (Exception ex) {
                ErrorInfo.IsOpen = true;
                ErrorInfo.Message = ex.Message;
            } finally {
                this.IsEnabled = true;
            }
        }
    }
}
