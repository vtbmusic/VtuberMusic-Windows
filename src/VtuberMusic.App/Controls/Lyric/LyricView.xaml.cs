using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.UI;
using System;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.Core.Models.Lyric;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.Lyric {
    public sealed partial class LyricView : UserControl {
        private int nowLyricIndex = -1;

        public LyricView() {
            this.InitializeComponent();
        }

        private void toLyric(int index) {
            DispatcherHelper.TryRun(Dispatcher, delegate {
                try {
                    if (-1 < nowLyricIndex && nowLyricIndex < ViewModel.Lyric.Lyric.Length) {
                        var preItem = (LyricItem)((ListViewItem)LyricListView.ContainerFromIndex(nowLyricIndex)).ContentTemplateRoot;
                        preItem.Pass();
                    }

                    nowLyricIndex = index;
                    var nowItem = (LyricItem)((ListViewItem)LyricListView.ContainerFromIndex(nowLyricIndex)).ContentTemplateRoot;
                    nowItem.Show();
                    LyricListView.SmoothScrollIntoViewWithIndexAsync(index, ScrollItemPlacement.Center, false, true, 0, 100);
                } catch {

                }
            });
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e) {
            ViewModel.MediaPlaybackService.Position = TimeSpan.FromMilliseconds((e.ClickedItem as LyricWords).Origin.Timestamp.Ticks / 10000);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            GC.Collect();
        }

        private void LyricListView_Loaded(object sender, RoutedEventArgs e) {
            WeakReferenceMessenger.Default.Register(this, delegate (object s, PlaybackMusicChangedMessage message) {
                nowLyricIndex = -1;
            });

            WeakReferenceMessenger.Default.Register(this, delegate (object s, PlaybackPositionChangedMessage message) {
                if (ViewModel.Lyric == null) return;
                for (int i = 0; i != ViewModel.Lyric.Lyric.Length; i++) {
                    if (i == ViewModel.Lyric.Lyric.Length - 1 && ViewModel.Lyric.Lyric[i].Origin.Timestamp.Ticks / 10000 <= message.Value.Position.TotalMilliseconds) {
                        if (this.nowLyricIndex == i) return;
                        toLyric(i);
                        return;
                    }

                    if (ViewModel.Lyric.Lyric[i].Origin.Timestamp.Ticks / 10000 >= message.Value.Position.TotalMilliseconds) {
                        if (i - 1 < 0) return;
                        toLyric(i - 1);
                        return;
                    }
                }
            });
        }
    }
}
