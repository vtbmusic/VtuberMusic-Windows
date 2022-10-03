using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class TrackMusicDialog : UserControl {
    private ContentDialog contentDialog;
    private readonly TrackMusicDialogViewModel ViewModel = Ioc.Default.GetRequiredService<TrackMusicDialogViewModel>();

    public string[] MusicIds;

    public TrackMusicDialog() {
        this.InitializeComponent();
    }

    public async Task ShowDialogAsync(string[] musicIds) {
        this.MusicIds = musicIds;

        contentDialog = new ContentDialog() {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Title = "添加到歌单",
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            IsPrimaryButtonEnabled = false,
            IsSecondaryButtonEnabled = false,
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Close,
            Content = this
        };

        await contentDialog.ShowAsync();
    }

    private async void FavoriteMusicButton_Click(object sender, RoutedEventArgs e) {
        await ViewModel.TrackMusicAsync("fav", MusicIds, true);
        contentDialog.Hide();
    }

    private async void ListView_ItemClick(object sender, ItemClickEventArgs e) {
        await ViewModel.TrackMusicAsync((e.ClickedItem as Playlist).id, MusicIds);
        contentDialog.Hide();
    }
}
