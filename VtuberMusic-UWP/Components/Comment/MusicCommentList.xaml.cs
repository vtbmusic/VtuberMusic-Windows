using VtuberMusic_UWP.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Comment {
    /// <summary>
    /// 音乐评论列表
    /// </summary>
    public sealed partial class MusicCommentList : UserControl {
        private AccountService account => App.Client.Account;
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Models.VtuberMusic.v1.Comment[]), typeof(MusicCommentList), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( d as MusicCommentList ).ItemsSourceChanged(e);
        }

        private void ItemsSourceChanged(DependencyPropertyChangedEventArgs e) {
            CommentList.ItemsSource = (Models.VtuberMusic.v1.Comment[])e.NewValue;
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public Models.VtuberMusic.v1.Comment[] ItemsSource {
            get { return (Models.VtuberMusic.v1.Comment[])this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public MusicCommentList() {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            CommentList.ItemsSource = this.ItemsSource;
        }
    }
}
