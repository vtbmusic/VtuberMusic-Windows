using MetroLog;
using MetroLog.Targets;
using Microsoft.AppCenter;
using System;
using System.Diagnostics;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Player
{
    public sealed partial class Playlist : UserControl
    {
        public Playlist()
        {
            this.InitializeComponent();

            App.Player.NowPlayingMusicChanged += NowPlayingMusicChanged;
            App.Player.PlayList.CollectionChanged += PlayList_CollectionChanged;
            DataView.ItemsSource = App.Player.PlayList;

            if (App.Player.PlayList.Count == 0)
            {
                None.Visibility = Visibility.Visible;
            }
        }

        private void PlayList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (App.Player.PlayList.Count != 0)
            {
                None.Visibility = Visibility.Collapsed;
            }
            else
            {
                None.Visibility = Visibility.Visible;
            }
        }

        private void NowPlayingMusicChanged(object sender, Music e)
        {
            if (App.Player.PlayList.Count == 0) None.Visibility = Visibility.Visible;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            App.Player.PlayList.Remove((Music)DataView.SelectedItem);
        }

        private void DeleteMusic_Click(object sender, RoutedEventArgs e)
        {
            App.Player.PlayList.Remove((Music)((AppBarButton)sender).Tag);
        }

        private void DeleteSwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            App.Player.PlayList.Remove((Music)args.SwipeControl.Tag);
        }

        private void DataView_Loaded(object sender, RoutedEventArgs e)
        {
            DataView.ScrollIntoView(App.Player.NowPlayingMusic, ScrollIntoViewAlignment.Leading);
        }
    }
}
