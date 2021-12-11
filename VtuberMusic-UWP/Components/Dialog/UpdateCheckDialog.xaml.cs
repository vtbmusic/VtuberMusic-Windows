using System;
using VtuberMusic_UWP.Models.Main;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class UpdateCheckDialog : UserControl, IContentDialogControl {
        private UpdateCheck info;
        public ContentDialog ContentDialog { get; private set; } = new ContentDialog();

        public UpdateCheckDialog(UpdateCheck updateInfo) {
            this.info = updateInfo;
            this.InitializeComponent();

            this.ContentDialog.Title = $"发现新版本 - v{ info.version }";
            this.ContentDialog.PrimaryButtonClick += this.ContentDialog_PrimaryButtonClick;
            this.ContentDialog.IsPrimaryButtonEnabled = true;
            this.ContentDialog.PrimaryButtonText = "前往外部链接更新";
            this.ContentDialog.CloseButtonText = "取消";
            this.ContentDialog.Content = this;
            this.ContentDialog.DefaultButton = ContentDialogButton.Primary;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await Launcher.LaunchUriAsync(new Uri(info.url));
        }
    }
}
