using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using VtuberMusic.App.Helper;
using VtuberMusic.App.ViewModels;
using VtuberMusic.AppCore.Messages;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class MusicPlayer : UserControl {
    private bool isPositionMove = false;
    private bool isVolumeMove = false;

    public event EventHandler RequsetShowPlaying;

    private readonly MusicPlayerViewModel ViewModel = Ioc.Default.GetRequiredService<MusicPlayerViewModel>();

    public MusicPlayer() {
        InitializeComponent();

        PositionSlider.AddHandler(PointerPressedEvent, new PointerEventHandler(PositionSlider_PointerPressed), true);
        PositionSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(PositionSlider_PointerReleased), true);

        VolumeSlider.AddHandler(PointerPressedEvent, new PointerEventHandler(VolumeSlider_PointerPressed), true);
        VolumeSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(VolumeSlider_PointerReleased), true);

        isPositionMove = true;
        isVolumeMove = true;

        VolumeSlider.Value = ViewModel.Volume;
        PositionSlider.Value = ViewModel.PlayerPosition.TotalSeconds;
        PositionSlider.Maximum = ViewModel.PlayerDuration.TotalSeconds;

        isPositionMove = false;
        isVolumeMove = false;
    }

    #region 处理歌曲进度条事件
    private void PositionSlider_PointerPressed(object sender, PointerRoutedEventArgs e) => isPositionMove = true;

    private void PositionSlider_PointerReleased(object sender, PointerRoutedEventArgs e) {
        if (isPositionMove) {
            ViewModel.PlayerPosition = TimeSpan.FromSeconds(PositionSlider.Value);
        }

        isPositionMove = false;
    }
    #endregion

    #region 音量条事件
    private void VolumeSlider_PointerReleased(object sender, PointerRoutedEventArgs e) => isVolumeMove = false;
    private void VolumeSlider_PointerPressed(object sender, PointerRoutedEventArgs e) => isVolumeMove = true;

    private void VolumeSlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e) {
        if (isVolumeMove)
            ViewModel.Volume = e.NewValue;
    }
    #endregion

    private void ShowPlaying_Click(object sender, RoutedEventArgs e) => RequsetShowPlaying?.Invoke(this, null);

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
        ViewModel.IsActive = true;

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackVolumeChangedMessage message) {
            DispatcherHelper.TryRun(delegate {
                if (!isVolumeMove) {
                    VolumeSlider.Value = message.Value;
                }
            });
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPositionChangedMessage message) {
            DispatcherHelper.TryRun(delegate {
                if (!isPositionMove) {
                    PositionSlider.Value = message.Value.Position.TotalSeconds;
                    PositionSlider.Maximum = message.Value.Duration.TotalSeconds;
                }
            });
        });
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
