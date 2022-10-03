using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.ViewModels.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class CreatePlaylistDialog : UserControl {
    private readonly CreatePlaylistDialogViewModel ViewModel = Ioc.Default.GetRequiredService<CreatePlaylistDialogViewModel>();
    private ContentDialog contentDialog;

    public CreatePlaylistDialog() {
        this.InitializeComponent();
    }

    public async Task ShowDialogAsync() {
        contentDialog = new ContentDialog() {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = "创建歌单",
            PrimaryButtonCommand = ViewModel.CreatePlaylistCommand,
            PrimaryButtonText = "创建",
            CloseButtonText = "取消",
            Content = this,
            DefaultButton = ContentDialogButton.Primary,
        };

        await contentDialog.ShowAsync();
    }
}
