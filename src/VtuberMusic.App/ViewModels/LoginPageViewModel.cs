using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public class LoginPageViewModel : AppViewModel {
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetService<IAuthorizationService>();

    public string Username { get => username; set => SetProperty(ref username, value); }
    private string username;
    public string Password { get => password; set => SetProperty(ref password, value); }
    private string password;

    public IAsyncRelayCommand LoginCommand { get; }
    public IAsyncRelayCommand PrivacyDialogCommand { get; }

    public LoginPageViewModel() {
        LoginCommand = new AsyncRelayCommand(LoingAsync);
        PrivacyDialogCommand = new AsyncRelayCommand(async delegate () {
            ContentDialog dialog = new() {
                Title = "VtuberMusic 隐私协议",
                PrimaryButtonText = "确认",
                DefaultButton = ContentDialogButton.Primary,
                Content = new PrivacyContentDialog()
            };

            var result = await dialog.ShowAsync();
        });
    }

    private async Task LoingAsync() {
        try {
            _ = await _authorizationService.LoginAsync(Username, Password);
            _ = App.RootFrame.Navigate(typeof(MainPage));
        } catch {

        }
    }
}
