using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Player
{
    public sealed partial class Playlist : UserControl
    {
        private Popup popup = new Popup();
        private Service.Player Player = App.Player;

        public Playlist()
        {
            this.InitializeComponent();
            Player.PlayListChanged += playListChanged;
            App.ViewModel.MainPage.SizeChanged += MainPage_SizeChanged;
            if (App.Player.PlayList.Count == 0)
            {
                None.Visibility = Visibility.Visible;
            }
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Height = e.NewSize.Height;
            Width = e.NewSize.Width;
        }

        private void playListChanged(object sender, EventArgs e)
        {
            if (App.Player.PlayList.Count == 0)
            {
                None.Visibility = Visibility.Visible;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            App.Player.PlayListDeleteMusic((MusicData)((Button)sender).Tag);
        }
        
        public void ShowPopup()
        {
            popup.Child = this;
            Height = Window.Current.Bounds.Height;
            Width = Window.Current.Bounds.Width;

            popup.IsOpen = true;
            PopupIn.Begin();
        }

        private void Background_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PopupOut.Completed += PopupOut_Completed;
            PopupOut.Begin();
        }

        private void PopupOut_Completed(object sender, object e)
        {
            popup.IsOpen = false;
        }
    }
}
