using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
