using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class EditAlbumInfoDialog : UserControl {
        public ContentDialog dialog = new ContentDialog();

        public EditAlbumInfoDialog() {
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示 Dialog
        /// </summary>
        /// <param name="id">歌单 id</param>
        /// <returns></returns>
        public async Task ShowAsync(string id) {
            this.dialog.Title = "编辑歌单信息";
            this.dialog.PrimaryButtonText = "确定";
            this.dialog.IsPrimaryButtonEnabled = false;
            this.dialog.CloseButtonText = "取消";
            this.dialog.DefaultButton = ContentDialogButton.Primary;
            this.dialog.Content = this;

            if (await this.dialog.ShowAsync() == ContentDialogResult.Primary) {
                await App.Client.Account.EditAlbumInfo(id, this.AlbumName.Text, this.Description.Text);
                App.ViewModel.MainPage.Load();
            }
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) =>
            this.dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);
    }
}
