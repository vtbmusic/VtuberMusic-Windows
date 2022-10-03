using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class ConfirmDeletePlaylistDialog : UserControl {
    private readonly ConfirmDeletePlaylistDialogViewModel ViewModel = Ioc.Default.GetRequiredService<ConfirmDeletePlaylistDialogViewModel>();
    private ContentDialog contentDialog;

    public ConfirmDeletePlaylistDialog() {
        this.InitializeComponent();
    }

    public async Task ShowDialogAsync(Playlist playlist) {
        ViewModel.PlaylistDelete = playlist;

        contentDialog = new ContentDialog {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = "是否删除歌单",
            Content = this,
            PrimaryButtonCommand = ViewModel.ConfirmDeleteCommand,
            PrimaryButtonText = "删除",
            CloseButtonText = "取消"
        };

        await contentDialog.ShowAsync();
    }
}
