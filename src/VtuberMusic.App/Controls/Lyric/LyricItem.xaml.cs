using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using VtuberMusic.App.Messages;
using VtuberMusic.Core.Models.Lyric;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic.App.Controls.Lyric {
    public sealed partial class LyricItem : UserControl {
        public static readonly DependencyProperty WordsProperty =
            DependencyProperty.Register("Words", typeof(LyricWords), typeof(LyricItem), new PropertyMetadata(null));

        public LyricWords Words {
            get => (LyricWords)GetValue(WordsProperty);
            set => SetValue(WordsProperty, value);
        }

        public LyricItem() {
            this.InitializeComponent();

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, NowLyricChangedMessage message) {
                if (Array.IndexOf(message.Value.Lyric.Lyric, Words) == message.Value.NowLyricIndex) {
                    Show();
                } else {
                    Pass();
                }
            });
        }

        public void Show() {
            Blur.Amount = 0;
        }

        public void Pass() {
            Blur.Amount = 2;
        }
    }
}
