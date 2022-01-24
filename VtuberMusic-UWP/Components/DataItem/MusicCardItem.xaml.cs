﻿using System;
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
    }
}
