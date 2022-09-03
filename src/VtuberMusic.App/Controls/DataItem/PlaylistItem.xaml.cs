using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class PlaylistItem : UserControl {
    public static readonly DependencyProperty PlaylistProperty =
        DependencyProperty.Register("Playlist", typeof(Playlist), typeof(PlaylistItem), new PropertyMetadata(null));

    public static readonly DependencyProperty PlaylistTypeProperty =
        DependencyProperty.Register("PlaylistType", typeof(PlaylistType), typeof(PlaylistItem), new PropertyMetadata(PlaylistType.Playlist));

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
    }

    private void RootGrid_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Hover", true);

    private void RootGrid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Normal", true);

    private void NavigateToPlaylist_Click(object sender, RoutedEventArgs e) => NavigationHelper.Navigate<PlaylistPage>(new PlaylistPageArg { Playlist = Playlist, PlaylistType = PlaylistType });
}
