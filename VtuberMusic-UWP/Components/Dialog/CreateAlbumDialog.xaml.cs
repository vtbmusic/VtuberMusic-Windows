using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class CreateAlbumDialog : UserControl, IContentDialogControl {
        public ContentDialog ContentDialog { get; private set; } = new ContentDialog();

        public CreateAlbumDialog() {
            this.InitializeComponent();

            this.ContentDialog.Title = "创建新歌单";
            this.ContentDialog.IsPrimaryButtonEnabled = false;
            this.ContentDialog.PrimaryButtonText = "创建";
            this.ContentDialog.CloseButtonText = "取消";
            this.ContentDialog.DefaultButton = ContentDialogButton.Primary;
            this.ContentDialog.PrimaryButtonClick += this.ContentDialog_PrimaryButtonClick;
            this.ContentDialog.Content = this;
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) => this.ContentDialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await App.Client.Account.CreateAlbum(this.AlbumName.Text, this.IsPrivateAlbum.IsChecked.GetValueOrDefault());
            App.ViewModel.MainPage.Load();
        }
    }
}
