using System;
using VtuberMusic_UWP.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Main
{
    public sealed partial class TopPanel : UserControl
    {
        public TopPanel()
        {
            this.InitializeComponent();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(TitleBar);
            App.ViewModel.TopPanel = this;

            if (App.Client.Account.Logged) Avatar.UriSource = new Uri(App.Client.Account.Profile.avatarUrl);
        }
    }
}
