using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components.Lyric
{
    public sealed partial class LyricItem : UserControl
    {
        public static readonly DependencyProperty LyricProperty =
            DependencyProperty.Register("Lyric", typeof(Models.Lyric.Lyric), typeof(LyricItem), new PropertyMetadata(null,
                new PropertyChangedCallback(LyricChanged)));

        private static void LyricChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LyricItem)d).onLyricChanged((Models.Lyric.Lyric)e.NewValue);
        }

        public Models.Lyric.Lyric Lyric
        {
            get { return (Models.Lyric.Lyric)GetValue(LyricProperty); }
            set { SetValue(LyricProperty, value); }
        }

        private void onLyricChanged(Models.Lyric.Lyric lyric)
        {
            SourceLyric.Text = lyric.Source;
            TranslationLyric.Text = lyric.Translation;
        }

        public LyricItem()
        {
            this.InitializeComponent();
        }

        public void Show()
        {
            SourceLyric.Style = NowSourceLyric;
            TranslationLyric.Style = NowTranslationLyric;
        }

        public void Hide()
        {
            SourceLyric.Style = NormalSourceLyric;
            TranslationLyric.Style = NormalTranslationLyric;
        }
    }

    public class HideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null && value.GetType() == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(((string)value))) return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}
