﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls;
public sealed partial class LoadingCover : UserControl {
    public static readonly DependencyProperty IsLoadingProperty =
        DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingCover), new PropertyMetadata(false));
    public static new readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(UIElement), typeof(LoadingCover), new PropertyMetadata(null));

    public bool IsLoading {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public new UIElement Content {
        get => (UIElement)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public LoadingCover() {
        InitializeComponent();
    }
}
