using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class EditAlbumInfoDialog : UserControl
    {
        public ContentDialog dialog = new ContentDialog();

        public EditAlbumInfoDialog()
        {
            this.InitializeComponent();
        }

        public async Task ShowAsync(string id)
        {
            dialog.Title = "编辑歌单信息";
            dialog.PrimaryButtonText = "确定";
            dialog.IsPrimaryButtonEnabled = false;
            dialog.CloseButtonText = "取消";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = this;

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                await App.Client.Account.EditAlbumInfo(id, AlbumName.Text, Description.Text);
                App.ViewModel.MainPage.Load();
            }
        }

        private void AlbumName_TextChanged(object sender, TextChangedEventArgs e) =>
            dialog.IsPrimaryButtonEnabled = !string.IsNullOrWhiteSpace(AlbumName.Text);
    }
}
