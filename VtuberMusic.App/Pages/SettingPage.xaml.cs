using Microsoft.UI.Xaml.Controls;
using System;
using VtuberMusic.App.Dialogs;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages {
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page {
        public SettingPage() {
            this.InitializeComponent();
        }

        private async void PrivacyDialog_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
            var dialog = new ContentDialog();
            dialog.Title = "VtuberMusic 隐私协议";
            dialog.PrimaryButtonText = "确认";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new PrivacyContentDialog();

            var result = await dialog.ShowAsync();
        }
    }
}
