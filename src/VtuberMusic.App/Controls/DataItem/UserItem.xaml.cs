using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.DataItem {
    public sealed partial class UserItem : UserControl {
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register("Profile", typeof(Profile), typeof(UserItem), new PropertyMetadata(null));

        private readonly DataItemViewModel ViewModel = Ioc.Default.GetRequiredService<DataItemViewModel>();

        public Profile Profile {
            get => (Profile)GetValue(ProfileProperty);
            set => SetValue(ProfileProperty, value);
        }

        public UserItem() {
            this.InitializeComponent();
        }
    }
}
