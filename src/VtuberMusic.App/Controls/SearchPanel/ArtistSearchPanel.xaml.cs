﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.SearchPanel;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.SearchPanel;
public sealed partial class ArtistSearchPanel : UserControl {
    public static readonly DependencyProperty KeywordProperty =
        DependencyProperty.Register("Keyword", typeof(string), typeof(ArtistSearchPanel), new PropertyMetadata(null, new PropertyChangedCallback(KeywordChanged)));

    private readonly ArtistSearchPanelViewModel ViewModel = Ioc.Default.GetRequiredService<ArtistSearchPanelViewModel>();

    private static void KeywordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ArtistSearchPanel).ViewModel.Keyword = e.NewValue as string;

    public string Keyword {
        get => GetValue(KeywordProperty) as string;
        set => SetValue(KeywordProperty, value);
    }

    public ArtistSearchPanel() {
        InitializeComponent();
    }
}
