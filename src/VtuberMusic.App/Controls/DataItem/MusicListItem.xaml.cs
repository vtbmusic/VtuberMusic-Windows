using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class MusicListItem : UserControl {
    public static readonly DependencyProperty MusicProperty =
        DependencyProperty.Register("Music", typeof(Music), typeof(MusicListItem), new PropertyMetadata(null, new PropertyChangedCallback(MusicPropertyChanged)));

    private readonly DataItemViewModel ViewModel = Ioc.Default.GetRequiredService<DataItemViewModel>();

    private static void MusicPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var musicItem = d as MusicListItem;
        musicItem.ArtistMenuFlyoutItem.Items.Clear();

        foreach (var item in musicItem.Music.artists) {
            musicItem.ArtistMenuFlyoutItem.Items.Add(new MenuFlyoutItem { Text = item.name.origin, Command = musicItem.ViewModel.NavigateToArtistCommand, CommandParameter = item });
        }
    }

    public Music Music {
        get => GetValue(MusicProperty) as Music;
        set => SetValue(MusicProperty, value);
    }

    public MusicListItem() {
        InitializeComponent();
    }
}
