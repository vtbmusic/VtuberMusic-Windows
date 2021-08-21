using System;
using System.Reflection;
using System.Runtime;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Settings : Page
    {
        private const string web = "https://vtbmusic.com/";
        private const string bilibili = "https://space.bilibili.com/8003519/";
        private const string qq = "https://jq.qq.com/?_wv=1027&k=qLjO1BCu";
        private const string twitter = "https://twitter.com/VtuberMusic";
        private const string email = "";

        public Settings()
        {
            this.InitializeComponent();
            Version.Text = "VtuberMusic | v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "-" + getGitCommitInfo();

            Username.Text = App.Client.Account.Account.userName;
            Nickname.Text = App.Client.Account.Profile.nickname;
            Avatar.ProfilePicture = new BitmapImage(new Uri(App.Client.Account.Profile.avatarUrl));

            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                FeadBackCenter.Visibility = Visibility.Visible;
            }
        }

        private string getGitCommitInfo()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
            if (attributes.Length != 0) return ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;

            return "Nan";
        }

        private void ForceGC_Click(object sender, RoutedEventArgs e)
        {
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration);
        }

        private async void EditProfile_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("https://vtbmusic.com/user"));

        private void LogOut_Click(object sender, RoutedEventArgs e) { }

        private async void BiliBili_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(bilibili));
        private async void QQ_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(qq));
        private async void Twitter_Click(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri(twitter));
        private async void FeadBackCenter_Click(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }

    public class BlurBackgroundPixelConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double) return (int)(double)value / 4;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
