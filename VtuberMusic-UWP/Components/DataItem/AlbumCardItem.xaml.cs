using PropertyChanged;
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
    public partial class AlbumCardItem : UserControl {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Album), typeof(AlbumCardItem), new PropertyMetadata(null));

        public Album Source {
            get { return (Album)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public AlbumCardItem() {
            this.InitializeComponent();
        }
    }
}
