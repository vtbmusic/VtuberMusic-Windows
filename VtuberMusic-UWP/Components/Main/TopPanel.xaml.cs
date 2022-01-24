using VtuberMusic_UWP.Pages;
using VtuberMusic_UWP.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Main {
    /// <summary>
    /// 顶部面板
    /// </summary>
    public sealed partial class TopPanel : UserControl {
        private AccountService account => App.Client.Account;

        public TopPanel() {
            this.InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                if (string.IsNullOrWhiteSpace(this.SearchKeyword.Text)) return;
                if (App.ViewModel.MainPage.ContentFrame.Content.GetType() == typeof(Search)) {
                    ( (Search)App.ViewModel.MainPage.ContentFrame.Content ).SearchKeyword(this.SearchKeyword.Text);
                    return;
                }

                App.ViewModel.NavigateToPage(typeof(Search), this.SearchKeyword.Text);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Window.Current.SetTitleBar(this.TitleBar);
            App.ViewModel.TopPanel = this;
        }

        private void SearchKeyword_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
            if (string.IsNullOrWhiteSpace(this.SearchKeyword.Text)) return;
            if (App.ViewModel.MainPage.ContentFrame.Content.GetType() == typeof(Search)) {
                ( (Search)App.ViewModel.MainPage.ContentFrame.Content ).SearchKeyword(this.SearchKeyword.Text);
                return;
            }

            App.ViewModel.NavigateToPage(typeof(Search), this.SearchKeyword.Text);
        }
    }
}
