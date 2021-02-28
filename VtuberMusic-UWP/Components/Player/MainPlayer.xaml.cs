using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using VtuberMusic_UWP.Pages;
using VtuberMusic_UWP.Service;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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
            player.DurationChanged += durationChanged;
            player.PlayStateChanged += playStateChanged;
            player.VolumeChanged += volumeChanged;
            Volume.Value = player.Volume;
            App.ViewModel.MainPlayer = this;
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

        private async void playStateChanged(object sender, PlayState e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
            {
                switch (player.PlayState)
                {
                    case PlayState.Pause:
                        Play.Content = "\ue613";
                        break;
                    case PlayState.Playing:
                        Play.Content = "\ue614";
                        break;
                }
            }));
        }

        private async void durationChanged(object sender, TimeSpan e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate
            {
                DurationText.Text = App.Player.Duration.GetValueOrDefault().ToString("mm\\:ss");
                DurationSlider.Maximum = App.Player.Duration.GetValueOrDefault().TotalMilliseconds;
                DurationSlider.Value = App.Player.Position.Milliseconds;
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
                }));
            }
        }

        private void update()
        {
            var data = player.NowPlayingMusic;
            if (data != null)
            {
                var image = new BitmapImage();
                image.UriSource = new Uri(data.ResourcesUrl.CoverImg);
                CoverImg.Source = image;

                MusicName.Text = data.OriginName;
                VocalName.Text = data.VocalName;
            }
            else
            {
                CoverImg.Source = null;
                MusicName.Text = "";
                VocalName.Text = "";
            }
        }

        private async void nowPlayingMusicChange(object sender, MusicData e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(update));
        }

        private void PlayList_Click(object sender, RoutedEventArgs e)
        {
            var playlist = new Playlist();
            playlist.ShowPopup();
        }

        private void DurationSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            App.Player.Position = TimeSpan.FromMilliseconds(DurationSlider.Value);
            DurationSliderIsDrag = false;
        }

        private void DurationSlider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DurationSliderIsDrag = true;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            switch (player.PlayState)
            {
                case Service.PlayState.Pause:
                    
                    Play.Content = "\ue613";
                    player.Play();
                    break;
                case PlayState.Playing:
                    Play.Content = "\ue614";
                    player.Pause();
                    break;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            player.Next();
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            player.Previous();
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            player.Volume = Volume.Value;
        }

        private void Volume_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            VolSliderIsDrag = true;
        }

        private void Volume_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            VolSliderIsDrag = false;
        }

        private void MusicInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.ViewModel.NavigateToPage(typeof(Playing));
        }
    }
}
