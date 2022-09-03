﻿using Microsoft.UI.Xaml.Controls;
using System;
using VtuberMusic.App.Dialogs;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class SettingPage : Page {
    public SettingPage() {
        InitializeComponent();
    }

    private async void PrivacyDialog_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        ContentDialog dialog = new() {
            Title = "VtuberMusic 隐私协议",
            PrimaryButtonText = "确认",
            DefaultButton = ContentDialogButton.Primary,
            Content = new PrivacyContentDialog()
        };
        _ = await dialog.ShowAsync();
    }
}
