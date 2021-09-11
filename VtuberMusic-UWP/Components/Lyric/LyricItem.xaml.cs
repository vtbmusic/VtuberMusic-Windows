using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Components.Lyric {
    /// <summary>
    /// 歌词 Item
    /// </summary>
    public sealed partial class LyricItem : UserControl {
        public static readonly DependencyProperty LyricProperty =
            DependencyProperty.Register("Lyric", typeof(Models.Lyric.Lyric), typeof(LyricItem), new PropertyMetadata(null,
                new PropertyChangedCallback(LyricChanged)));

        private Style NowSourceLyric => (Style)Application.Current.Resources["NowSourceLyric"];
        private Style NowTranslationLyric => (Style)Application.Current.Resources["NowTranslationLyric"];
        private Style NormalSourceLyric => (Style)Application.Current.Resources["NormalSourceLyric"];
        private Style NormalTranslationLyric => (Style)Application.Current.Resources["NormalTranslationLyric"];

        private static void LyricChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( (LyricItem)d ).onLyricChanged((Models.Lyric.Lyric)e.NewValue);
        }

        /// <summary>
        /// 歌词 Lyric Object
        /// </summary>
        public Models.Lyric.Lyric Lyric {
            get { return (Models.Lyric.Lyric)this.GetValue(LyricProperty); }
            set { this.SetValue(LyricProperty, value); }
        }

        private void onLyricChanged(Models.Lyric.Lyric lyric) {
            this.SourceLyric.Text = lyric.Source;
            this.TranslationLyric.Text = lyric.Translation;
        }

        public LyricItem() {
            this.InitializeComponent();
        }

        /// <summary>
        /// 高亮当前歌词
        /// </summary>
        public void Show() {
            this.SourceLyric.Style = this.NowSourceLyric;
            this.TranslationLyric.Style = this.NowTranslationLyric;
        }

        /// <summary>
        /// 隐藏当前歌词
        /// </summary>
        public void Hide() {
            this.SourceLyric.Style = this.NormalSourceLyric;
            this.TranslationLyric.Style = this.NormalTranslationLyric;
        }
    }
}
