using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Main
{
    public partial class BlockTitle : UserControl
    {
        public static readonly DependencyProperty TextProperty =
     DependencyProperty.Register("Text", typeof(string), typeof(BlockTitle), new PropertyMetadata("Text"));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public BlockTitle()
        {
            this.InitializeComponent();
        }
    }
}
