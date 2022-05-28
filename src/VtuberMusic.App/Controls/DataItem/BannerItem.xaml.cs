using VtuberMusic.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.DataItem {
    public sealed partial class BannerItem : UserControl {
        public static readonly DependencyProperty BannerProperty =
            DependencyProperty.Register("Banner", typeof(Banner), typeof(BannerItem), new PropertyMetadata(null));

        public Banner Banner {
            get { return GetValue(BannerProperty) as Banner; }
            set { SetValue(BannerProperty, value); }
        }

        public BannerItem() {
            this.InitializeComponent();
        }
    }
}
