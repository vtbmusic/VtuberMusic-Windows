using System;
using System.Reflection;
using System.Runtime;
using VtuberMusic_UWP.Service;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 设置页
    /// </summary>
    public sealed partial class Settings : Page {
        private const string web = "https://vtbmusic.com/";
        private const string bilibili = "https://space.bilibili.com/8003519/";
        private const string qq = "https://jq.qq.com/?_wv=1027&k=qLjO1BCu";
        private const string twitter = "https://twitter.com/VtuberMusic";
        private const string email = "";

        private AccountService account => App.Client.Account;

        public Settings() {
            this.InitializeComponent();
            this.Version.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " & " + this.getGitCommitInfo();

            //if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported()) {
            //    this.FeadBackCenter.Visibility = Visibility.Visible;
            //}

            Theme.SelectedIndex = (int)App.RoamingSettings.Theme;
        }

        private string getGitCommitInfo() {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
            return attributes.Length != 0 ? ( (AssemblyInformationalVersionAttribute)attributes[0] ).InformationalVersion : "Nan";
        }

        private void ForceGC_Click(object sender, RoutedEventArgs e) {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration);
        }

        private async void EditProfile_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://vtbmusic.com/user"));

        private void LogOut_Click(object sender, RoutedEventArgs e) { }

        private async void BiliBili_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(bilibili));
        private async void QQ_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(qq));
        private async void Twitter_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(twitter));
        private async void FeadBackCenter_Click(object sender, RoutedEventArgs e) {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private async void Tencent_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://wj.qq.com/s2/9062724/e2a8/"));

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e) => App.RoamingSettings.Theme = (ElementTheme)Theme.SelectedIndex;
    }

    public class BlurBackgroundPixelConvert : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value is double ? (int)(double)value / 4 : (object)null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return null;
        }
    }
}
