﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.SearchPanel {
    public sealed partial class ArtistSearchPanel : UserControl {
        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register("Keyword", typeof(string), typeof(ArtistSearchPanel), new PropertyMetadata(null));

        public string Keyword {
            get => GetValue(KeywordProperty) as string;
            set => SetValue(KeywordProperty, value);
        }

        public ArtistSearchPanel() {
            this.InitializeComponent();
        }
    }
}
