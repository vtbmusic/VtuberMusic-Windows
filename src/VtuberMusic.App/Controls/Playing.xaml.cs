using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Messages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls {
    public sealed partial class Playing : UserControl {
        public event EventHandler RequestClosePlaying;
        private bool isMove = false;

        public Playing() {
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackPositionChangedMessage message) {
                DispatcherHelper.TryRun(delegate {
                    if (!isMove) {
                        PositionSlider.Value = message.Value.Position.TotalSeconds;
                        PositionSlider.Maximum = message.Value.Duration.TotalSeconds;
                    }
                });
            });

            PositionSlider.AddHandler(PointerPressedEvent, new PointerEventHandler(Slider_PointerPressed), true);
            PositionSlider.AddHandler(PointerReleasedEvent, new PointerEventHandler(Slider_PointerReleased), true);
        }

        private void Slider_PointerPressed(object sender, PointerRoutedEventArgs e) {
            isMove = true;
        }

        private void Slider_PointerReleased(object sender, PointerRoutedEventArgs e) {
            if (isMove) ViewModel.MediaPlaybackService.Position = TimeSpan.FromSeconds(PositionSlider.Value);
            isMove = false;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            RequestClosePlaying?.Invoke(this, null);
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e) {
            CoverImgViewBox.Height = e.NewSize.Height;
            CoverImgViewBox.Width = e.NewSize.Height;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            ViewModel.IsActive = true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            ViewModel.IsActive = false;
        }
    }
}
