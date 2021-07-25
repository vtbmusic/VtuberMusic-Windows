using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace VtuberMusic_UWP.Pages.SetupPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Account : Page, ISetupStep
    {
        public object Icon { get; private set; } = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 128, Glyph = "\uE13D" };

        public Account()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            Username.IsEnabled = false;
            Password.IsEnabled = false;
            Next.IsEnabled = false;
            Back.IsEnabled = false;
            LoadingBar.Visibility = Visibility.Visible;

            try
            {
                await App.Client.Account.Login(Username.Text, Password.Password);
                Frame.Navigate(typeof(Finsh), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
            catch (Exception ex)
            {
                Username.IsEnabled = true;
                Password.IsEnabled = true;
                Next.IsEnabled = true;
                Back.IsEnabled = true;
                LoadingBar.Visibility = Visibility.Collapsed;

                ErrorInfoBar.IsOpen = true;
                ErrorInfoBar.Message = ex.Message;
            }
        }

        private void Username_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(Username.Text) && !string.IsNullOrWhiteSpace(Password.Password))
            {
                Next.IsEnabled = true;
            }
        }

        private void Password_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(Username.Text) && !string.IsNullOrWhiteSpace(Password.Password))
            {
                Next.IsEnabled = true;
            }
        }
    }
}
