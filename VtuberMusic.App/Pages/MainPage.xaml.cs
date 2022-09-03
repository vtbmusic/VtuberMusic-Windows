using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.Models;
using VtuberMusic.App.Services;
using VtuberMusic.App.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page {
        private MainPageViewModel viewModel => this.DataContext as MainPageViewModel;
        private INavigationService navigationService => viewModel.NavigationService;

        public MainPage() {
            this.InitializeComponent();
            viewModel.NavigationService.SetContentFrame(MainFrame);
        }

        private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
            if (args.IsSettingsSelected) {
                navigationService.Navigate<SettingPage>();
                return;
            }

            if (args.SelectedItem == null) return;
            var tag = (args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItemBase).Tag as NavigationTag;
            navigationService.Navigate(tag.Type, tag.Args);
        }

        private void NavigationView_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args) {
            navigationService.RequestGoBack();
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
            if (e.SourcePageType == typeof(SettingPage)) {
                MainNavigationView.SelectedItem = MainNavigationView.SettingsItem;
                return;
            }

            var tag = new NavigationTag { Type = e.SourcePageType, Args = e.Parameter };

            foreach (var item in viewModel.NavigationItems) {
                if (item.Tag != null && (item.Tag as NavigationTag).Type == tag.Type && (item.Tag as NavigationTag).Args == tag.Args) {
                    MainNavigationView.SelectedItem = item;
                    return;
                }
            }

            foreach (var footerItem in viewModel.PaneFooterNavigationItems) {
                if (footerItem.Tag != null && (footerItem.Tag as NavigationTag).Type == tag.Type && (footerItem.Tag as NavigationTag).Args == tag.Args) {
                    MainNavigationView.SelectedItem = footerItem;
                    return;
                }
            }

            MainNavigationView.SelectedItem = null;
        }

        private void MusicPlayer_RequsetShowPlaying(object sender, System.EventArgs e) {
            viewModel.IsPlayingShow = true;
            PlayingTransfrom.Y = ActualHeight;
            PlayingIn.Begin();
        }

        private void Page_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e) {
            viewModel.PageHeight = e.NewSize.Height;
            if (!viewModel.IsPlayingShow && PlayingTransfrom != null) PlayingTransfrom.Y = e.NewSize.Height;
        }

        private void PlayingControl_RequestClosePlaying(object sender, System.EventArgs e) {
            PlayingOut.Begin();
            PlayingOut.Completed += delegate {
                viewModel.IsPlayingShow = false;
            };
        }
    }
}
