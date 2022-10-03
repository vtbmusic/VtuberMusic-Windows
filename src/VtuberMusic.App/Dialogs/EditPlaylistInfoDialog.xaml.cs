using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class EditPlaylistInfoDialog : UserControl {
    private readonly EditPlaylistInfoDialogViewModel ViewModel = Ioc.Default.GetRequiredService<EditPlaylistInfoDialogViewModel>();
    private ContentDialog contentDialog;

    public EditPlaylistInfoDialog() {
        this.InitializeComponent();
    }

    public async Task ShowDialogAsync(Playlist playlist) {
        ViewModel.Playlist = playlist;
        contentDialog = new ContentDialog {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = $"修改歌单 {playlist.name} 的信息",
            PrimaryButtonCommand = ViewModel.EditPlaylistCommand,
            PrimaryButtonText = "修改",
            CloseButtonText = "取消",
            Content = this
        };

        await contentDialog.ShowAsync();
    }
}
