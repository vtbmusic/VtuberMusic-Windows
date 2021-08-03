using VtuberMusic_UWP.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Components.Main
{
    public sealed partial class TopPanel : UserControl
    {
        public TopPanel()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(TitleBar);
            App.ViewModel.TopPanel = this;
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (string.IsNullOrWhiteSpace(SearchKeyword.Text)) return;
                if (App.ViewModel.MainPage.ContentFrame.Content.GetType() == typeof(Search))
                {
                    ((Search)App.ViewModel.MainPage.ContentFrame.Content).SearchKeyword(SearchKeyword.Text);
                    return;
                }

                App.ViewModel.NavigateToPage(typeof(Search), SearchKeyword.Text);
            }
        }

        private void PersonPicture_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
