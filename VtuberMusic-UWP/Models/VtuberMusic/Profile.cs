using PropertyChanged;
using System.ComponentModel;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class Profile : INotifyPropertyChanged {
        public string userId { get; set; }
        public int userType { get; set; }
        public string nickname { get; set; }
        public int avatarImgId { get; set; }
        public string avatarUrl { get; set; }
        public int backgroundImgId { get; set; }
        public string backgroundUrl { get; set; }
        public string level { get; set; }
        public int? experience { get; set; }
        public int? nextexperience { get; set; }
        public string signature { get; set; }
        public int createTime { get; set; }
        public string userName { get; set; }
        public int accountType { get; set; }
        public int? birthday { get; set; }
        public int authStatus { get; set; }
        public string description { get; set; }
        public string remarkName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [AddINotifyPropertyChangedInterface]
    public class ProfileResponse : INotifyPropertyChanged {
        public Profile profile { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
