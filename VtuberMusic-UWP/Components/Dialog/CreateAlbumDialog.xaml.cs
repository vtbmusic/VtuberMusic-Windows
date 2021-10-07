using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class CreateAlbumDialog : ContentDialog {
        public CreateAlbumDialog() {
            this.InitializeComponent();
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) => this.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await App.Client.Account.CreateAlbum(this.AlbumName.Text, this.IsPrivateAlbum.IsChecked.GetValueOrDefault());
            App.ViewModel.MainPage.Load();
        }
    }
}
