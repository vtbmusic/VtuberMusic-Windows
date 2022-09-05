using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Serilog.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.ViewModels.Pages;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Helper;
using Windows.ApplicationModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class SettingPage : Page {
    public SettingsPageViewModel ViewModel = Ioc.Default.GetRequiredService<SettingsPageViewModel>();

    public SettingPage() {
        InitializeComponent();

        var version = Package.Current.Id.Version;
        VersionText.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision} © VtuberMusic {DateTimeOffset.Now.Year}";
    }

        private async void PrivacyDialog_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
        ContentDialog dialog = new() {
            XamlRoot = this.XamlRoot,
            Title = "VtuberMusic 隐私协议",
            PrimaryButtonText = "确认",
            DefaultButton = ContentDialogButton.Primary,
            Content = new PrivacyContentDialog()
        };
        await dialog.ShowAsync();
    }

    private void DefaultNavigationPageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
        SettingsHelper.DefaultNavigationPage = (DefaultNavigationPage)DefaultNavigationPageSelector.SelectedValue;

    private void DefaultNavigationPageSelector_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        => DefaultNavigationPageSelector.SelectedIndex =
        ViewModel.DefaultNavigationPageType.ToList().FindIndex(item => item.Value == SettingsHelper.DefaultNavigationPage);
}