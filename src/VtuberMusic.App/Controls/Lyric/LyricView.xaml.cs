using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using VtuberMusic.App.Helper;
using VtuberMusic.App.ViewModels.Lyric;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models.Lyric;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.Lyric;
public sealed partial class LyricView : UserControl {
    private int nowLyricIndex = -1;
    private LyricItem nowLyricItem = null;

    private readonly LyricViewViewModel ViewModel = Ioc.Default.GetRequiredService<LyricViewViewModel>();
    private readonly IMediaPlayBackService _mediaPlayBackService = Ioc.Default.GetRequiredService<IMediaPlayBackService>();

    public LyricView() {
        InitializeComponent();
    }

    private void toLyric(int index) => DispatcherHelper.TryRun(async delegate {
        try {
            if (-1 < nowLyricIndex && nowLyricIndex < ViewModel.Lyric.Lyric.Length && nowLyricItem != null) {
                nowLyricItem.Pass();
            }

            nowLyricIndex = index;
            await LyricListView.SmoothScrollIntoViewWithIndexAsync(index, ScrollItemPlacement.Center, false, true, 0, 100);
            if (LyricListView.ContainerFromIndex(nowLyricIndex) != null) {
                nowLyricItem = (LyricItem)((ListViewItem)LyricListView.ContainerFromIndex(nowLyricIndex)).ContentTemplateRoot;
                nowLyricItem.Show();
            }
        } catch {

        }
    });

    private void ListView_ItemClick(object sender, ItemClickEventArgs e) => _mediaPlayBackService.Position = TimeSpan.FromMilliseconds((e.ClickedItem as LyricWords).Origin.Timestamp.Ticks / 10000);

    private void UserControl_Unloaded(object sender, RoutedEventArgs e) {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        ViewModel.IsActive = false;
        GC.Collect();
    }

    private void LyricListView_Loaded(object sender, RoutedEventArgs e) {
        WeakReferenceMessenger.Default.Register(this, delegate (object s, PlaybackMusicChangedMessage message) {
            nowLyricIndex = -1;
            nowLyricItem = null;
        });

        WeakReferenceMessenger.Default.Register(this, delegate (object s, PlaybackPositionChangedMessage message) {
            if (ViewModel.Lyric == null) {
                return;
            }

            for (var i = 0; i != ViewModel.Lyric.Lyric.Length; i++) {
                if (i == ViewModel.Lyric.Lyric.Length - 1 && ViewModel.Lyric.Lyric[i].Origin.Timestamp.Ticks / 10000 <= message.Value.Position.TotalMilliseconds) {
                    if (nowLyricIndex == i) {
                        return;
                    }

                    toLyric(i);
                    return;
                }

                if (ViewModel.Lyric.Lyric[i].Origin.Timestamp.Ticks / 10000 >= message.Value.Position.TotalMilliseconds) {
                    if (i - 1 < 0) {
                        return;
                    }

                    toLyric(i - 1);
                    return;
                }
            }
        });
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) => ViewModel.IsActive = true;
}
