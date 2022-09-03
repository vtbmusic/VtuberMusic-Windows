using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Pages;
public partial class LoginPageViewModel : ObservableObject {
    private readonly IAuthorizationService _authorizationService;

    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private string password;

    public LoginPageViewModel(IAuthorizationService authorizationService) {
        _authorizationService = authorizationService;
    }

    [RelayCommand]
    public async Task Login() {
        try {
            await _authorizationService.LoginAsync(this.Username, this.Password);
            App.RootFrame.Navigate(typeof(MainPage));
        } catch {

        }
    }
}
