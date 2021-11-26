using VtuberMusic_UWP.Models.VtuberMusic;
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
            DependencyProperty.Register("ItemsSource", typeof(CommentResult), typeof(MusicCommentList), new PropertyMetadata(null, new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( d as MusicCommentList ).ItemsSourceChanged(e);
        }

        private void ItemsSourceChanged(DependencyPropertyChangedEventArgs e) {
            if (e.NewValue == null) {
                CommentList.ItemsSource = null;
                return;
            }

            CommentList.ItemsSource = ((CommentResult)e.NewValue).comments;
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public CommentResult ItemsSource {
            get { return (CommentResult)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public MusicCommentList() {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (this.ItemsSource == null) {
                CommentList.ItemsSource = null;
                return;
            }

            CommentList.ItemsSource = this.ItemsSource.comments;
        }
    }
}