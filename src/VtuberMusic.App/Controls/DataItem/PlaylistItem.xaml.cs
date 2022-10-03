using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class PlaylistItem : UserControl {
    public static readonly DependencyProperty PlaylistProperty =
        DependencyProperty.Register("Playlist", typeof(Playlist), typeof(PlaylistItem), new PropertyMetadata(null, OnPlaylistPropertyChanged));

    public static readonly DependencyProperty PlaylistTypeProperty =
        DependencyProperty.Register("PlaylistType", typeof(PlaylistType), typeof(PlaylistItem), new PropertyMetadata(PlaylistType.Playlist));

    private readonly DataItemViewModel ViewModel = Ioc.Default.GetRequiredService<DataItemViewModel>();

    public Playlist Playlist {
        get => GetValue(PlaylistProperty) as Playlist;
        set => SetValue(PlaylistProperty, value);
    }

    public PlaylistType PlaylistType {
        get => (PlaylistType)GetValue(PlaylistTypeProperty);
        set => SetValue(PlaylistTypeProperty, value);
    }

    public PlaylistItem() {
        InitializeComponent();

        ViewModel.UpdatePlaylistCanEdit(this.Playlist, this.PlaylistType);
    }

    private static void OnPlaylistPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = d as PlaylistItem;
        control.ViewModel.UpdatePlaylistCanEdit(control.Playlist, control.PlaylistType);
    }

    private void RootGrid_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Hover", true);

    private void RootGrid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Normal", true);

    private void NavigateToPlaylist_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = Playlist, PlaylistType = PlaylistType });

    private async void DeletePlaylistMenuFlyoutItem_Click(object sender, RoutedEventArgs e) =>
        await new ConfirmDeletePlaylistDialog().ShowDialogAsync(this.Playlist);
}
