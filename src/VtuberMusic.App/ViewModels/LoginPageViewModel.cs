using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic.App.Dialogs;
using VtuberMusic.App.Pages;
using VtuberMusic.Core.Services;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic.App.ViewModels {
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
                var dialog = new ContentDialog();
                dialog.Title = "VtuberMusic 隐私协议";
                dialog.PrimaryButtonText = "确认";
                dialog.DefaultButton = ContentDialogButton.Primary;
                dialog.Content = new PrivacyContentDialog();

                var result = await dialog.ShowAsync();
            });
        }

        private async Task LoingAsync() {
            try {
                await _authorizationService.LoginAsync(Username, Password);
                ((App)App.Current).RootFrame.Navigate(typeof(MainPage));
            } catch (Exception ex) {

            }
        }
    }
}
