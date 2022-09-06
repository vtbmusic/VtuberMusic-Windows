using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using VtuberMusic.App.ViewModels;
using VtuberMusic.AppCore.Services;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class MusicPlayer : UserControl {
    private bool isMove = false;

    public event EventHandler RequsetShowPlaying;

    private readonly MusicPlayerViewModel ViewModel = Ioc.Default.GetRequiredService<MusicPlayerViewModel>();
    private readonly IMediaPlayBackService _mediaPlayBackService = Ioc.Default.GetRequiredService<IMediaPlayBackService>();

    public MusicPlayer() {
        InitializeComponent();

        PositionSlider.AddHandler(PointerPressedEvent, new PointerEventHandler(Slider_PointerPressed), true);
        PositionSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(Slider_PointerReleased), true);
    }

    private void MediaPlaybackService_PositionChanged(object sender, TimeSpan e) => this.DispatcherQueue.TryEnqueue(delegate {
        try {
            if (PositionSlider != null & !isPositionMove) {
            PositionSlider.Value = e.TotalSeconds;
        }
        } catch { }
    });

    private void Slider_PointerPressed(object sender, PointerRoutedEventArgs e) => isMove = true;

    private void Slider_PointerReleased(object sender, PointerRoutedEventArgs e) {
        if (isMove) {
            _mediaPlayBackService.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        isMove = false;
    }

    private void ShowPlaying_Click(object sender, RoutedEventArgs e) => RequsetShowPlaying?.Invoke(this, null);

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        _mediaPlayBackService.PositionChanged += MediaPlaybackService_PositionChanged;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
        _mediaPlayBackService.PositionChanged -= MediaPlaybackService_PositionChanged;
    }
}
