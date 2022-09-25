using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Helper;
using VtuberMusic.App.PageArgs;
using VtuberMusic.App.Pages;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Enums;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class MusicItem : UserControl {
    public static readonly DependencyProperty MusicProperty =
        DependencyProperty.Register("Music", typeof(Music), typeof(MusicItem), new PropertyMetadata(null, new PropertyChangedCallback(MusicPropertyChanged)));

    private readonly DataItemViewModel ViewModel = Ioc.Default.GetRequiredService<DataItemViewModel>();

    public Music Music {
        get => GetValue(MusicProperty) as Music;
        set => SetValue(MusicProperty, value);
    }

    public MusicItem() {
        InitializeComponent();
    }

    private static void MusicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var musicItem = d as MusicItem;
        musicItem.ArtistMenuFlyoutItem.Items.Clear();

        foreach (var item in musicItem.Music.artists) {
            musicItem.ArtistMenuFlyoutItem.Items.Add(new MenuFlyoutItem { Text = item.name.origin, Command = musicItem.ViewModel.NavigateToArtistCommand, CommandParameter = item });
        }
    }

    private void Grid_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Hover", true);

    private void Grid_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => VisualStateManager.GoToState(this, "Normal", true);

    private async void TrackMusicMenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
        await new TrackMusicDialog().ShowDialogAsync(new string[] { this.Music.id });
    }

    private void ShowCommentMenuFlyoutItem_Click(object sender, RoutedEventArgs e) =>
        NavigationHelper.Navigate<CommentPage>(new CommentPageArg { Type = CommentContentType.song, Music = this.Music });
}
