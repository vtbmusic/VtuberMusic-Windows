﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;
using VtuberMusic.App.Models;
using VtuberMusic.App.Services;
using VtuberMusic.App.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic.App.Pages;
/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class MainPage : Page {
    private MainPageViewModel viewModel => DataContext as MainPageViewModel;
    private INavigationService _navigationService = Ioc.Default.GetRequiredService<INavigationService>();

    public MainPage() {
        InitializeComponent();
        _navigationService.SetContentFrame(MainFrame);
    }

    private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args) {
        if (args.IsSettingsSelected) {
            _navigationService.Navigate<SettingPage>();
            return;
        }

        if (args.SelectedItem == null) {
            return;
        }

        var tag = (args.SelectedItem as NavigationViewItemBase).Tag as NavigationTag;
        _navigationService.Navigate(tag.Type, tag.Args);
    }

    private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _navigationService.RequestGoBack();

    private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
        if (e.SourcePageType == typeof(SettingPage)) {
            MainNavigationView.SelectedItem = MainNavigationView.SettingsItem;
            return;
        }

        NavigationTag tag = new() { Type = e.SourcePageType, Args = e.Parameter };
        foreach (var item in from item in this.viewModel.NavigationItems
                             where item.Tag != null && (item.Tag as NavigationTag).Type == tag.Type && (item.Tag as NavigationTag).Args == tag.Args
                             select item) {
            MainNavigationView.SelectedItem = item;
            return;
        }

        foreach (var footerItem in from footerItem in this.viewModel.PaneFooterNavigationItems
                                   where footerItem.Tag != null && (footerItem.Tag as NavigationTag).Type == tag.Type && (footerItem.Tag as NavigationTag).Args == tag.Args
                                   select footerItem) {
            MainNavigationView.SelectedItem = footerItem;
            return;
        }

        MainNavigationView.SelectedItem = null;
    }

    private void MusicPlayer_RequsetShowPlaying(object sender, System.EventArgs e) {
        this.viewModel.IsPlayingShow = true;
        PlayingTransfrom.Y = this.ActualHeight;
        PlayingIn.Begin();
    }

    private void Page_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e) {
        this.viewModel.PageHeight = e.NewSize.Height;
        if (!this.viewModel.IsPlayingShow && PlayingTransfrom != null) {
            PlayingTransfrom.Y = e.NewSize.Height;
        }
    }

    private void PlayingControl_RequestClosePlaying(object sender, System.EventArgs e) {
        PlayingOut.Begin();
        PlayingOut.Completed += delegate {
            this.viewModel.IsPlayingShow = false;
        };
    }
}
