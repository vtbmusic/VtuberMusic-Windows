using CommunityToolkit.Mvvm.Messaging.Messages;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Messages {
    public class AuthorizationStateChangedMessage : ValueChangedMessage<AccountProfileResponse> {
        public AuthorizationStateChangedMessage(AccountProfileResponse value) : base(value) {
        }
    }
}
