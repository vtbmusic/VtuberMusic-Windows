using System;
using VtuberMusic_UWP.Models.Main;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class UpdateCheckDialog : ContentDialog {
        private UpdateCheck info;

        public UpdateCheckDialog(UpdateCheck updateInfo) {
            this.info = updateInfo;
            this.InitializeComponent();
            this.Title = $"发现新版本 - v{ info.version }";
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await Launcher.LaunchUriAsync(new Uri(info.url));
        }
    }
}
