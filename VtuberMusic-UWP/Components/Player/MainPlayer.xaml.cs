using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Numerics;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Pages;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace VtuberMusic_UWP.Components.Player {
    /// <summary>
    /// 主播放器控件
    /// </summary>
    public sealed partial class MainPlayer : UserControl {
        private bool DurationSliderIsDrag = false;
        private bool VolSliderIsDrag = false;

        private Service.Player player {
            get {
                return App.Player;
            }
        }

        public MainPlayer() {
            this.InitializeComponent();
            this.player.NowPlayingMusicChanged += this.nowPlayingMusicChange;
            this.player.PositionChanged += this.positionChanged;
            this.player.PlayStateChanged += this.playStateChanged;
            this.player.VolumeChanged += this.volumeChanged;
            this.Volume.Value = this.player.Volume;
            App.ViewModel.MainPlayer = this;

            if (this.player.NowPlayingMusic == null) this.Visibility = Visibility.Collapsed;
            this.ShareShadow.Receivers.Add(this.ShadowBackground);
            this.CoverImgGrid.Translation = new Vector3(0, 0, 32);
        }

        private async void volumeChanged(object sender, double e) {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                if (!this.VolSliderIsDrag) {
                    this.Volume.Value = e;
                }
            }));
        }

        private async void playStateChanged(object sender, MediaPlaybackState e) {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                switch (e) {
                    case MediaPlaybackState.Paused:
                        this.PlayButtonIocn.Symbol = Symbol.Play;
                        break;
                    case MediaPlaybackState.Playing:
                        this.PlayButtonIocn.Symbol = Symbol.Pause;
                        break;
                }
            }));
        }

        private async void positionChanged(object sender, TimeSpan e) {
            if (!this.DurationSliderIsDrag) {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                    this.PositionText.Text = App.Player.Position.ToString("mm\\:ss");
                    this.DurationSlider.Value = App.Player.Position.TotalMilliseconds;

                    this.DurationText.Text = App.Player.Duration.ToString("mm\\:ss");
                    this.DurationSlider.Maximum = App.Player.Duration.TotalMilliseconds;
                }));
            }
        }

        private void update() {
            var data = this.player.NowPlayingMusic;
            if (data != null) {
                var image = new BitmapImage() { DecodePixelHeight = 55, DecodePixelWidth = 90 };
                this.CoverImg.ImageSource = image;
                image.UriSource = new Uri(data.picUrl);

                this.MusicName.Text = data.name;
                this.Vocal.ItemsSource = data.artists;
                this.LikeMusicIcon.Glyph = data.like ? "\uE00B" : "\uE006";
            }

            this.DurationText.Text = App.Player.Duration.ToString("mm\\:ss");
            this.DurationSlider.Maximum = App.Player.Duration.TotalMilliseconds;
            this.DurationSlider.Value = App.Player.Position.Milliseconds;
        }

        private async void nowPlayingMusicChange(object sender, Music e) {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate {
                this.update();
            }));
        }

        private void DurationSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e) {
            App.Player.Position = TimeSpan.FromMilliseconds(this.DurationSlider.Value);
            this.DurationSliderIsDrag = false;
        }

        private void DurationSlider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) => this.DurationSliderIsDrag = true;

        private void Play_Click(object sender, RoutedEventArgs e) {
            switch (this.player.PlayState) {
                case MediaPlaybackState.Paused:
                    this.PlayButtonIocn.Symbol = Symbol.Play;
                    this.player.Play();
                    break;
                case MediaPlaybackState.Playing:
                    this.PlayButtonIocn.Symbol = Symbol.Pause;
                    this.player.Pause();
                    break;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e) => this.player.Next();
        private void Prev_Click(object sender, RoutedEventArgs e) => this.player.Previous();
        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => this.player.Volume = this.Volume.Value;
        private void Volume_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) => this.VolSliderIsDrag = true;
        private void Volume_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) => this.VolSliderIsDrag = false;

        private void MusicInfo_Tapped(object sender, TappedRoutedEventArgs e) {
            if (App.RootFrame.Content.GetType() != typeof(Playing)) {
                App.RootFrame.Navigate(typeof(Playing), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
            }
        }

        private async void LikeMusicButton_Click(object sender, RoutedEventArgs e) {
            this.LikeMusicButton.IsEnabled = false;

            try {
                await App.Client.Account.LikeMusic(this.player.NowPlayingMusic.id, !this.player.NowPlayingMusic.like);
                this.player.NowPlayingMusic.like = !this.player.NowPlayingMusic.like;

                this.LikeMusicIcon.Glyph = this.player.NowPlayingMusic.like ? "\uE00B" : "\uE006";

                this.LikeMusicButton.IsEnabled = true;
            } catch (Exception ex) {
                this.LikeMusicButton.IsEnabled = true;
                InfoBarPopup.Show("无法喜欢音乐", ex.Message);

                var data = new Dictionary<string, string>()
                {
                    { "Music_Id", this.player.NowPlayingMusic.id },
                    { "Like", (!this.player.NowPlayingMusic.like).ToString() }
                };

                Crashes.TrackError(ex, data);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e) => App.ViewModel.NavigateToPage(typeof(Pages.Artist), ( (HyperlinkButton)sender ).Tag);
    }

    public class PercentagesConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            if (value != null && value.GetType() == typeof(double)) {
                var convertValue = (double)value;
                return ( (int)( convertValue * 100 ) ).ToString() + "%";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class TimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string culture) {
            if (value != null && value.GetType() == typeof(double)) {
                var convertValue = (double)value;
                return TimeSpan.FromMilliseconds(convertValue).ToString("mm\\:ss");
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
