using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class MusicItem : UserControl, INotifyPropertyChanged {
        public static readonly DependencyProperty MusicProperty =
            DependencyProperty.Register("Music", typeof(Music), typeof(MusicItem), new PropertyMetadata(null, new PropertyChangedCallback(MusicChanged)));

        private static void MusicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( (MusicItem)d ).PropertyChanged?.Invoke(d, new PropertyChangedEventArgs("Music"));
        }

        public Music Music {
            get { return (Music)this.GetValue(MusicProperty); }
            set { this.SetValue(MusicProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MusicItem() {
            this.InitializeComponent();
        }
    }
}
