using System;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class CreateAlbumDialog : UserControl {
        private ContentDialog dialog = new ContentDialog();

        public CreateAlbumDialog() {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示 Dialog
        /// </summary>
        public async void ShowAsync() {
            this.dialog.Title = "创建歌单";
            this.dialog.PrimaryButtonText = "创建";
            this.dialog.IsPrimaryButtonEnabled = false;
            this.dialog.CloseButtonText = "取消";
            this.dialog.DefaultButton = ContentDialogButton.Primary;
            this.dialog.Content = this;

            if (await this.dialog.ShowAsync() == ContentDialogResult.Primary) {
                await App.Client.Account.CreateAlbum(this.AlbumName.Text, this.IsPrivateAlbum.IsChecked.GetValueOrDefault());
                App.ViewModel.MainPage.Load();
            }
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) => this.dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);
    }
}
