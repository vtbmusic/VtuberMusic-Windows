using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using VtuberMusic.App.PageArgs;
using VtuberMusic.AppCore.Enums;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Firends : Page {
        public Firends() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if (e.Parameter is FirendsPageArg arg) {
                ViewModel.Profile = arg.Profile;
                switch (arg.Type) {
                    case FirendsPageType.Fans:
                        Nav.SelectedItem = FansItem;
                        break;
                    case FirendsPageType.Follwers:
                        Nav.SelectedItem = SubItem;
                        break;
                    default:
                        Nav.SelectedItem = SubItem;
                        break;
                }
            }
        }

        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            hideAll();
            if (args.SelectedItem == FansItem) {
                ViewModel.IsFansShow = true;
            } else if (args.SelectedItem == SubItem) {
                ViewModel.IsFollwerdsShow = true;
            }
        }

        private void hideAll() {
            ViewModel.IsFansShow = false;
            ViewModel.IsFollwerdsShow = false;
        }
    }
}
