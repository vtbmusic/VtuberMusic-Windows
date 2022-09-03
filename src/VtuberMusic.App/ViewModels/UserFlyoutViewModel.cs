using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using VtuberMusic.Core.Messages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public partial class UserFlyoutViewModel : ObservableRecipient {
    private readonly IAuthorizationService _authorizationService;

    public UserFlyoutViewModel(IAuthorizationService authorizationService) {
        _authorizationService = authorizationService;
    }

    [ObservableProperty]
    private Profile profile;

    public UserFlyoutViewModel() {
        this.Profile = _authorizationService.Profile;

        WeakReferenceMessenger.Default.Register<AuthorizationStateChangedMessage>(this, (r, m) => {
            this.Profile = m.Value.profile;
        });
    }
}
