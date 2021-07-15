using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Search : Page
    {
        private string keyWord = "";

        public Search()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            keyWord = (string)e.Parameter;
        }

        private async void Music_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MusicLoading.Visibility = Visibility.Visible;
            MusicDataList.ItemsSource = (await App.Client.SearchMusic(keyWord)).Data;
            MusicLoading.Visibility = Visibility.Collapsed;
        }
    }
}
