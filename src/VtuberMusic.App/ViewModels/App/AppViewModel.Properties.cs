using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using VtuberMusic.App.Services;
using VtuberMusic.AppCore.Services;
using Windows.UI.Xaml;

namespace VtuberMusic.App.ViewModels {
    public partial class AppViewModel {
        public Thickness MusicPlayerHeight = new Thickness(0, 0, 0, 90);

        public INavigationService NavigationService = Ioc.Default.GetService<INavigationService>();
        public IMediaPlayBackService MediaPlaybackService = Ioc.Default.GetService<IMediaPlayBackService>();

        public static AppViewModel Instance { get; } = new Lazy<AppViewModel>(() => new AppViewModel()).Value;
    }
}
