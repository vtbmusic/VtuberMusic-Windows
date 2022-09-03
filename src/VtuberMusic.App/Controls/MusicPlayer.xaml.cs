using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using VtuberMusic.App.ViewModels;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class MusicPlayer : UserControl {
    private bool isMove = false;

    public event EventHandler RequsetShowPlaying;

    public MusicPlayer() {
        InitializeComponent();
        (DataContext as MusicPlayerViewModel).MediaPlaybackService.PositionChanged += MediaPlaybackService_PositionChanged;

        PositionSlider.AddHandler(PointerPressedEvent, new PointerEventHandler(Slider_PointerPressed), true);
        PositionSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(Slider_PointerReleased), true);
    }

    private void MediaPlaybackService_PositionChanged(object sender, TimeSpan e) => _ = DispatcherQueue.TryEnqueue(delegate {
        if (!isMove) {
            PositionSlider.Value = e.TotalSeconds;
        }
    });

    private void Slider_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) => isMove = true;

    private void Slider_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
        if (isMove) {
            (DataContext as MusicPlayerViewModel).MediaPlaybackService.Position = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        isMove = false;
    }

    private void ShowPlaying_Click(object sender, RoutedEventArgs e) => RequsetShowPlaying?.Invoke(this, null);
}
