using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.ViewModels.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class EditProfileDialog : UserControl {
    private EditProfileDialogViewModel ViewModel = Ioc.Default.GetRequiredService<EditProfileDialogViewModel>();
    private ContentDialog contentDialog;

    public EditProfileDialog() {
        this.InitializeComponent();

        ViewModel.PropertyChanged += delegate {
            contentDialog.IsPrimaryButtonEnabled = !ViewModel.HasErrors;
        };
    }

    public async Task ShowDialogAsync() {
        contentDialog = new ContentDialog {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = $"修改个人信息",
            PrimaryButtonCommand = ViewModel.EditProfileCommand,
            DefaultButton = ContentDialogButton.Primary,
            PrimaryButtonText = "修改",
            CloseButtonText = "取消",
            Content = this
        };

        contentDialog.IsPrimaryButtonEnabled = !ViewModel.HasErrors;
        await contentDialog.ShowAsync();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

    }
}
