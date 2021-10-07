using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class EditAlbumInfoDialog : ContentDialog {
        private string albumId;

        public EditAlbumInfoDialog(string id) {
            this.InitializeComponent();
            this.albumId = id;
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) =>
            this.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(this.AlbumName.Text);

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await App.Client.Account.EditAlbumInfo(albumId, this.AlbumName.Text, this.Description.Text);
            App.ViewModel.MainPage.Load();
        }
    }
}
