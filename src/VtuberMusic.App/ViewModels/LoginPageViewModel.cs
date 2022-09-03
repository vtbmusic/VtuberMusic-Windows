using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public partial class LoginPageViewModel : ObservableObject {
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetService<IAuthorizationService>();

    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private string password;

    public LoginPageViewModel() {
    }

    [RelayCommand]
    public async void PrivacyDialog() {
        ContentDialog dialog = new() {
            Title = "VtuberMusic 隐私协议",
            PrimaryButtonText = "确认",
            DefaultButton = ContentDialogButton.Primary,
            Content = new PrivacyContentDialog()
        };

        await dialog.ShowAsync();
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
