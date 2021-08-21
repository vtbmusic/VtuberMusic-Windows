using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Dialog
{
    public sealed partial class CreateAlbumDialog : UserControl
    {
        private ContentDialog dialog = new ContentDialog();

        public CreateAlbumDialog()
        {
            this.InitializeComponent();
        }

        public async void ShowAsync()
        {
            dialog.Title = "创建歌单";
            dialog.PrimaryButtonText = "创建";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = this;

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                await App.Client.Account.CreateAlbum(AlbumName.Text, IsPrivateAlbum.IsChecked.GetValueOrDefault());
                App.ViewModel.MainPage.Load();
            }
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) => dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(AlbumName.Text);
    }
}
