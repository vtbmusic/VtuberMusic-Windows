using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VtuberMusic_UWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Profile : Page
    {
        public Profile()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter.GetType() == typeof(Models.VtuberMusic.Profile))
            {
                var profile = (Models.VtuberMusic.Profile)e.Parameter;

                Nickname.Text = profile.nickname;
                LevelText.Text = $"Lv.{ profile.level.ToString() }";
                LevelExp.Value = profile.experience.GetValueOrDefault();
                LevelExp.Maximum = profile.nextexperience.GetValueOrDefault();

                Avatar.ProfilePicture = new BitmapImage(new Uri(profile.avatarUrl));
            }
        }
    }
}
