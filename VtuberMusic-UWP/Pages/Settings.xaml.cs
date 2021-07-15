using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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
        }

        private string getGitCommitInfo()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length != 0) return ((AssemblyFileVersionAttribute)attributes[0]).Version;

            return "Nan";
        }

        private void ForceGC_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect(100, GCCollectionMode.Forced, true);
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
