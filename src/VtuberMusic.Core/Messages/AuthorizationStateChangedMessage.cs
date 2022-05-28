using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using VtuberMusic.Core.Models;

namespace VtuberMusic.Core.Messages {
    public class AuthorizationStateChangedMessage : ValueChangedMessage<AccountProfileResponse> {
        public AuthorizationStateChangedMessage(AccountProfileResponse value) : base(value) {
        }
    }
}
