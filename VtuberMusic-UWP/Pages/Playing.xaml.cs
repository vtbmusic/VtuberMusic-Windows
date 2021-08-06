using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using VtuberMusic_UWP.Components.Lyric;
using VtuberMusic_UWP.Models.Lyric;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Playing : Page
    {
        private bool loaded = false;
        private Lyric[] lyrics;
        private LyricItem nowLyricItem = null;
        private DispatcherTimer lyricTimer = new DispatcherTimer();
        private bool canUpdatePosition = true;
        private int tickMs = 200;

        public Playing()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            ShareShadow.Receivers.Add(ShadowBackground);
            CoverImgGrid.Translation = new Vector3(0, 0, 32);

            lyricTimer.Interval = TimeSpan.FromMilliseconds(tickMs);
            lyricTimer.Tick += lyricTick;

            switch (App.Player.PlayState)
            {
                case MediaPlaybackState.Playing:
                    PlayButtonIcon.Symbol = Symbol.Pause;
                    break;
                default:
                    PlayButtonIcon.Symbol = Symbol.Play;
                    break;
            }

            App.Player.NowPlayingMusicChanged += async delegate
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate
                {
                    stopTick();
                    init();
                }));
            };

            App.Player.PlayStateChanged += async delegate (object sender, MediaPlaybackState e)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate
                {
                    if (e == MediaPlaybackState.Playing && App.Player.NowPlayingMusic != null && lyrics != null && App.RootFrame.Content.GetType() == typeof(Playing))
                    {
                        startTick();
                    }
                    else
                    {
                        stopTick();
                    }

                    switch (e)
                    {
                        case MediaPlaybackState.Playing:
                            PlayButtonIcon.Symbol = Symbol.Pause;
                            break;
                        default:
                            PlayButtonIcon.Symbol = Symbol.Play;
                            break;
                    }
                }));
            };

            App.Player.PositionChanged += async delegate (object sender, TimeSpan e)
            {
                if (!canUpdatePosition) return;
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                {
                    if (App.RootFrame.Content.GetType() != typeof(Playing)) return;

                    NowPlayTime.Text = App.Player.Position.ToString("mm\\:ss");
                    Position.Value = App.Player.Position.TotalMilliseconds;

                    Duration.Text = App.Player.Duration.ToString("mm\\:ss");
                    Position.Maximum = App.Player.Duration.TotalMilliseconds;
                });
            };

            init();
        }

        public async void init()
        {
            if (App.Player.NowPlayingMusic == null) return;

            string artist = "";
            foreach (var item in App.Player.NowPlayingMusic.artists)
            {
                artist += item.name.origin + " ";
            }

            MusicName.Text = App.Player.NowPlayingMusic.name;
            Artist.Text = artist;

            var image = new BitmapImage(new Uri(App.Player.NowPlayingMusic.picUrl));
            PageBackground.ImageSource = image;
            CoverImg.ImageSource = image;

            var client = new RestClient();
            var request = new RestRequest(App.Player.NowPlayingMusic.vrcUrl, Method.GET);
            var response = await client.ExecuteAsync(request);

            try
            {
                lyrics = await Task.Run<Models.Lyric.Lyric[]>( () => LyricPaser.Parse(response.Content));
                LyricView.ItemsSource = lyrics;

                startTick();
                loaded = true;
            }
            catch (Exception ex)
            {

            }
        }

        private async void startTick()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate
            {
                lyricTimer.Start();
            }));
        }

        private async void stopTick()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(delegate
            {
                lyricTimer.Stop();
            }));
        }

        private void lyricTick(object sender, object args)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 0; i != lyrics.Length; i++)
            {
                if (i == lyrics.Length - 1)
                {
                    ToLyric(i);
                    return;
                }

                Debug.WriteLine(App.Player.Position);
                if (lyrics[i].Time <= App.Player.Position && lyrics[i + 1].Time >= App.Player.Position)
                {
                    ToLyric(i);
                    return;
                }
            }

            ToLyric(-1);
            watch.Stop();
            Debug.WriteLine("{0}ms now:{1}", watch.ElapsedMilliseconds, App.Player.Position);
        }

        private void ToLyric(int index)
        {
            if (nowLyricItem != null) nowLyricItem.Hide();
            if (index == -1) return;
            UpdateLayout();

            var itemContainer = (UIElement)LyricView.ContainerFromItem(lyrics[index]);
            GeneralTransform generalTransform = LyricScrollViwer.TransformToVisual(itemContainer);
            Point point = generalTransform.TransformPoint(new Point());

            if (itemContainer == null) return;
            try
            {
                if (itemContainer is ContentPresenter)
                {
                    nowLyricItem = FindVisualChild<LyricItem>(itemContainer);
                    nowLyricItem.Show();
                }

                LyricScrollViwer.ChangeView(0,
                -point.Y + LyricScrollViwer.VerticalOffset - (LyricScrollViwer.ActualHeight / 3),
                null);
            }
            catch { }

            UpdateLayout();
        }

        private ChildType FindVisualChild<ChildType>(DependencyObject obj)
        where ChildType : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildType)
                {
                    return (child as ChildType);
                }
                else
                {
                    ChildType childOfChild = FindVisualChild<ChildType>(child);
                    if (childOfChild != null)
                        return (childOfChild);
                }
            }

            return (null);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            stopTick();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (loaded && App.Player.PlayStateInfo == MediaPlaybackState.Playing) startTick();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) => App.Player.Previous();

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            switch (App.Player.PlayState)
            {
                case MediaPlaybackState.Playing:
                    App.Player.Pause();
                    break;
                default:
                    App.Player.Play();
                    break;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) => App.Player.Next();

        private void Position_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            canUpdatePosition = false;
        }

        private void Position_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            App.Player.Position = TimeSpan.FromMilliseconds(Position.Value);
            canUpdatePosition = true;
        }

        private void Position_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            canUpdatePosition = true;
        }
    }
}
