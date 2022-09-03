using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.Core.Models.Lyric;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.Lyric;
public sealed partial class LyricItem : UserControl {
    public static readonly DependencyProperty WordsProperty =
        DependencyProperty.Register("Words", typeof(LyricWords), typeof(LyricItem), new PropertyMetadata(null));

    public LyricWords Words {
        get => (LyricWords)GetValue(WordsProperty);
        set => SetValue(WordsProperty, value);
    }

    public LyricItem() {
        InitializeComponent();
    }

    public void Show() => Blur.Amount = 0;

    public void Pass() => Blur.Amount = 2;
}
