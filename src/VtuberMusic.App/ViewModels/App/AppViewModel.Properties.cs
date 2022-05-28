using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using VtuberMusic.App.Services;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Models;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic.App.ViewModels {
    public partial class AppViewModel {
        public Thickness MusicPlayerHeight = new Thickness(0, 0, 0, 90);

        public INavigationService NavigationService = Ioc.Default.GetService<INavigationService>();
        public IMediaPlayBackService MediaPlaybackService = Ioc.Default.GetService<IMediaPlayBackService>();

        public static AppViewModel Instance { get; } = new Lazy<AppViewModel>(() => new AppViewModel()).Value;
    }
}
