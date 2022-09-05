using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using VtuberMusic.AppCore.Enums;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class SettingsPageViewModel : ObservableObject {
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty]
    private Dictionary<string, DefaultNavigationPage> defaultNavigationPageType = new Dictionary<string, DefaultNavigationPage>() {
        { "发现", DefaultNavigationPage.Home },
        { "资料库", DefaultNavigationPage.Library },
        { "我喜欢的音乐", DefaultNavigationPage.LikeMusic }
    };

    [ObservableProperty]
    private Account account;
    [ObservableProperty]
    private Profile profile;

    public SettingsPageViewModel(IAuthorizationService authorizationService) {
        _authorizationService = authorizationService;

        this.Account = _authorizationService.Account;
        this.profile = _authorizationService.Profile;
    }
}
