using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.ViewModels.Pages;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class Library : Page {
    private readonly LibraryViewModel ViewModel = Ioc.Default.GetRequiredService<LibraryViewModel>();

    public Library() {
        InitializeComponent();
    }

    private async void CreatePlaylist_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        await new CreatePlaylistDialog().ShowDialogAsync();

    private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        ViewModel.IsActive = true;

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) =>
        ViewModel.IsActive = false;
}
