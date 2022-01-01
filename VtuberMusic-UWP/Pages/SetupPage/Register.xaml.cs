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
    public sealed partial class Register : Page, ISetupStep {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE928" };

        public Register() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void Next_Click(object sender, RoutedEventArgs e) {
            this.Frame.Navigate(typeof(RegisterNickname),
                new SetupRegisterData { Username = Username.Text, Password = Password.Password},
                new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void Info_TextChanged(object sender, TextChangedEventArgs e) {
            Next.IsEnabled = Password.Password != "" && Username.Text != "";
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e) {
            Next.IsEnabled = Password.Password != "" && Username.Text != "";
        }

        private void Back_Click(object sender, RoutedEventArgs e) {
            this.Frame.GoBack();
        }
    }
}
