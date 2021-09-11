using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 账户资料页
    /// </summary>
    public sealed partial class Profile : Page {
        public Profile() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter.GetType() == typeof(Models.VtuberMusic.Profile)) {
                var profile = (Models.VtuberMusic.Profile)e.Parameter;

                this.Nickname.Text = profile.nickname;
                this.LevelText.Text = $"Lv.{ profile.level.ToString() }";
                this.LevelExp.Value = profile.experience.GetValueOrDefault();
                this.LevelExp.Maximum = profile.nextexperience.GetValueOrDefault();

                this.Avatar.ProfilePicture = new BitmapImage(new Uri(profile.avatarUrl));
            }
        }
    }
}
