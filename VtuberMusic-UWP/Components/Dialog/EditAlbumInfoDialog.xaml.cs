using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class EditAlbumInfoDialog : UserControl, IContentDialogControl {
        private string albumId;
        public ContentDialog ContentDialog { get; private set; } = new ContentDialog();

        public EditAlbumInfoDialog(string id) {
            this.InitializeComponent();
            this.albumId = id;

            this.ContentDialog.Title = "修改歌单名称";
            this.ContentDialog.IsPrimaryButtonEnabled = true;
            this.ContentDialog.PrimaryButtonText = "确定";
            this.ContentDialog.CloseButtonText = "取消";
            this.ContentDialog.DefaultButton = ContentDialogButton.Primary;
            this.ContentDialog.Content = this;
            this.ContentDialog.PrimaryButtonClick += this.ContentDialog_PrimaryButtonClick;
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) =>
            this.ContentDialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await App.Client.Account.EditAlbumInfo(albumId, this.AlbumName.Text, this.Description.Text);
            App.ViewModel.MainPage.Load();
        }
    }
}
