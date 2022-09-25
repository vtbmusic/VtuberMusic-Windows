using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.Comments;
public sealed partial class CommentView : UserControl {
    public static DependencyProperty MusicProperty =
        DependencyProperty.Register("Music", typeof(Music), typeof(CommentView), new PropertyMetadata(null, new PropertyChangedCallback(OnMusicPropertyChanged)));

    public static DependencyProperty PlaylistProperty =
        DependencyProperty.Register("Playlist", typeof(Playlist), typeof(CommentView), new PropertyMetadata(null, OnPlaylistPropertyChanged));

    public static DependencyProperty ContentTypeProperty =
        DependencyProperty.Register("ContentType", typeof(CommentContentType), typeof(CommentView), new PropertyMetadata(CommentContentType.song));

    public Music Music {
        get => (Music)GetValue(MusicProperty);
        set => SetValue(MusicProperty, value);
    }

    public Playlist Playlist {
        get => (Playlist)GetValue(PlaylistProperty);
        set => SetValue(PlaylistProperty, value);
    }

    public CommentContentType ContentType {
        get => (CommentContentType)GetValue(ContentTypeProperty);
        set => SetValue(ContentTypeProperty, value);
    }

    private readonly CommentViewModel ViewModel = Ioc.Default.GetRequiredService<CommentViewModel>();

    public CommentView() {
        this.InitializeComponent();
    }

    private static void OnMusicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = (d as CommentView);
        control.ViewModel.Music = (Music)e.NewValue;
    }

    private static void OnPlaylistPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var control = (d as CommentView);
        control.ViewModel.Playlist = (Playlist)e.NewValue;
    }
}
