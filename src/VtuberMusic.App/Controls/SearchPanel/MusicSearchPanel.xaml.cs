﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace VtuberMusic.App.Controls.SearchPanel {
    public sealed partial class MusicSearchPanel : UserControl {
        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register("Keyword", typeof(string), typeof(MusicSearchPanel), new PropertyMetadata(null, new PropertyChangedCallback(KeywordChanged)));

        private static void KeywordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            (d as MusicSearchPanel).ViewModel.Keyword = e.NewValue as string;
        }

        public string Keyword {
            get => GetValue(KeywordProperty) as string;
            set => SetValue(KeywordProperty, value);
        }

        public MusicSearchPanel() {
            this.InitializeComponent();
        }
    }
}
