using System;
using System.Collections.Generic;
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

namespace VtuberMusic_UWP.Components.DataItem {
    public sealed partial class MusicCardItem : UserControl {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Music), typeof(MusicCardItem), new PropertyMetadata(null));

        public Music Source {
            get { return (Music)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public MusicCardItem() {
            this.InitializeComponent();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e) {
            VisualStateManager.GoToState(this, "Hover", true);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e) {
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void FlyoutPlay_Click(object sender, RoutedEventArgs e) {
            App.Player.SetMusic(( sender as FrameworkElement ).Tag as Music);
        }

        private void FlyoutNext_Click(object sender, RoutedEventArgs e) {

        }

        private void FlyoutArtist_Loaded(object sender, RoutedEventArgs e) {

        }

        private void FlyoutShare_Click(object sender, RoutedEventArgs e) {

        }

        private void FlyoutAddTo_Loaded(object sender, RoutedEventArgs e) {

        }
    }
}
