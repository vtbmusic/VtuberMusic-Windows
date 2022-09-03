using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using VtuberMusic.Core.Messages;
using VtuberMusic.Core.Models;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels;
public class UserFlyoutViewModel : AppViewModel {
    private readonly IAuthorizationService _authorizationService = Ioc.Default.GetService<IAuthorizationService>();

    public Profile Profile { get => profile; set => SetProperty(ref profile, value); }
    private Profile profile;

    public UserFlyoutViewModel() {
        Profile = _authorizationService.Profile;

        WeakReferenceMessenger.Default.Register<AuthorizationStateChangedMessage>(this, (r, m) => {
            Profile = m.Value.profile;
        });
    }
}
