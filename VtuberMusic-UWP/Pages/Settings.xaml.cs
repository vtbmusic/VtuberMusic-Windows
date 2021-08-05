using System;
using System.Reflection;
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
        public Settings()
        {
            this.InitializeComponent();
            Version.Text = "VtuberMusic | v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "-" + getGitCommitInfo();

            Username.Text = App.Client.Account.Account.userName;
            Nickname.Text = App.Client.Account.Profile.nickname;
            Avatar.ProfilePicture = new BitmapImage(new Uri(App.Client.Account.Profile.avatarUrl));
        }

        private string getGitCommitInfo()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
            if (attributes.Length != 0) return ((AssemblyInformationalVersionAttribute)attributes[0]).InformationalVersion;

            return "Nan";
        }

        private void ForceGC_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private async void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://vtbmusic.com/"));
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {

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
