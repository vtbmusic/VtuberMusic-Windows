using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using VtuberMusic.AppCore.Enums;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class SettingsPageViewModel : ObservableObject {
    [ObservableProperty]
    private Dictionary<string, DefaultNavigationPage> defaultNavigationPageType = new Dictionary<string, DefaultNavigationPage>() {
        { "发现", DefaultNavigationPage.Home },
        { "资料库", DefaultNavigationPage.Library },
        { "我喜欢的音乐", DefaultNavigationPage.LikeMusic }
    };
}
