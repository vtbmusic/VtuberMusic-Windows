using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
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

namespace VtuberMusic_UWP.Components.Player
{
    public sealed partial class MainPlayer : UserControl
    {
        private bool DurationSliderIsDrag = false;
        private bool VolSliderIsDrag = false;

        private Service.Player player
        {
            get
            {
                return App.Player;
            }
        }

        public MainPlayer()
        {
            this.InitializeComponent();
            player.NowPlayingMusicChanged += nowPlayingMusicChange;
            player.PositionChanged += positionChanged;
            player.PlayStateChanged += playStateChanged;
            player.VolumeChanged += volumeChanged;
            Volume.Value = player.Volume;
            App.ViewModel.MainPlayer = this;

            if (player.NowPlayingMusic == null) this.Visibility = Visibility.Collapsed;
        }

        private async void volumeChanged(object sender, double e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
            {
                if (!VolSliderIsDrag)
                {
                    Volume.Value = e;
                }
            }));
        }

        private async void playStateChanged(object sender, MediaPlaybackState e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
            {
                switch (e)
                {
                    case MediaPlaybackState.Paused:
                        PlayButtonIocn.Symbol = Symbol.Play;
                        break;
                    case MediaPlaybackState.Playing:
                        PlayButtonIocn.Symbol = Symbol.Pause;
                        break;
                }
            }));
        }

        private async void positionChanged(object sender, TimeSpan e)
        {
            if (!DurationSliderIsDrag)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
                {
                    PositionText.Text = App.Player.Position.ToString("mm\\:ss");
                    DurationSlider.Value = App.Player.Position.TotalMilliseconds;

                    DurationText.Text = App.Player.Duration.ToString("mm\\:ss");
                    DurationSlider.Maximum = App.Player.Duration.TotalMilliseconds;
                }));
            }
        }

        private void update()
        {
            var data = player.NowPlayingMusic;
            if (data != null)
            {
                var image = new BitmapImage() { DecodePixelHeight = 55, DecodePixelWidth = 90 };
                CoverImg.ImageSource = image;
                image.UriSource = new Uri(data.picUrl);

                MusicName.Text = data.name;
                Vocal.ItemsSource = data.artists;
                if (data.like)
                {
                    LikeMusicIcon.Glyph = "\uE00B";
                }
                else
                {
                    LikeMusicIcon.Glyph = "\uE006";
                }
            }

            DurationText.Text = App.Player.Duration.ToString("mm\\:ss");
            DurationSlider.Maximum = App.Player.Duration.TotalMilliseconds;
            DurationSlider.Value = App.Player.Position.Milliseconds;
        }

        private async void nowPlayingMusicChange(object sender, Music e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
            {
                update();
            }));
        }

        private void DurationSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            App.Player.Position = TimeSpan.FromMilliseconds(DurationSlider.Value);
            DurationSliderIsDrag = false;
        }

        private void DurationSlider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) => DurationSliderIsDrag = true;

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            switch (player.PlayState)
            {
                case MediaPlaybackState.Paused:
                    PlayButtonIocn.Symbol = Symbol.Play;
                    player.Play();
                    break;
                case MediaPlaybackState.Playing:
                    PlayButtonIocn.Symbol = Symbol.Pause;
                    player.Pause();
                    break;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e) => player.Next();
        private void Prev_Click(object sender, RoutedEventArgs e) => player.Previous();
        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => player.Volume = Volume.Value;
        private void Volume_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) => VolSliderIsDrag = true;
        private void Volume_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) => VolSliderIsDrag = false;

        private void MusicInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.RootFrame.Content.GetType() != typeof(Playing))
            {
                App.RootFrame.Navigate(typeof(Playing), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom } );
            }
        }

        private async void LikeMusicButton_Click(object sender, RoutedEventArgs e)
        {
            LikeMusicButton.IsEnabled = false;

            try
            {
                await App.Client.Account.LikeMusic(player.NowPlayingMusic.id, !player.NowPlayingMusic.like);
                player.NowPlayingMusic.like = !player.NowPlayingMusic.like;

                if (player.NowPlayingMusic.like)
                {
                    LikeMusicIcon.Glyph = "\uE00B";
                }
                else
                {
                    LikeMusicIcon.Glyph = "\uE006";
                }

                LikeMusicButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                LikeMusicButton.IsEnabled = true;
                InfoBarPopup.Show("无法喜欢音乐", ex.Message);

                var data = new Dictionary<string, string>()
                {
                    { "Music_Id", player.NowPlayingMusic.id },
                    { "Like", (!player.NowPlayingMusic.like).ToString() }
                };

                Crashes.TrackError(ex, data);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e) => App.ViewModel.NavigateToPage(typeof(Pages.Artist), ((HyperlinkButton)sender).Tag);
    }

    public class PercentagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null && value.GetType() == typeof(double))
            {
                var convertValue = (double)value;
                return ((int)(convertValue * 100)).ToString() + "%";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null && value.GetType() == typeof(double))
            {
                var convertValue = (double)value;
                return TimeSpan.FromMilliseconds(convertValue).ToString("mm\\:ss");
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
