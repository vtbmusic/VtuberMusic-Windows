using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components
{
    public partial class MarqueeText : UserControl
    {
        public static readonly DependencyProperty TextProperty =
       DependencyProperty.Register("Text", typeof(string), typeof(MarqueeText), new PropertyMetadata("Text", new PropertyChangedCallback(TextChangedEvntHandler)));

        private Storyboard TextContentMove => this.Resources["TextContentMove"] as Storyboard;

        private static void TextChangedEvntHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MarqueeText)d).TextChange(e);
        }

        private protected void TextChange(DependencyPropertyChangedEventArgs args)
        {
            TextContent.Text = (string)args.NewValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public MarqueeText()
        {
            this.InitializeComponent();
        }

        private void TextContent_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TextContentMove.Seek(TextContentMove.Duration.TimeSpan * 2 - TextContentMove.GetCurrentTime());
        }

        private void TextContent_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var offest = -TextContent.ActualWidth + ActualWidth;

            if (offest < 0)
            {
                var current = TextContentMove.GetCurrentTime();

                ((DoubleAnimation)TextContentMove.Children[0]).To = offest;
                TextContentMove.Completed += TextContentMove_Completed;
                TextContentMove.Begin();


                if (current == null) return;
                if (current < TextContentMove.Duration.TimeSpan)
                {
                    TextContentMove.Seek(current);
                }
                else
                {
                    TextContentMove.Seek(current - TextContentMove.Duration.TimeSpan);
                }
            }
        }

        private void TextContentMove_Completed(object sender, object e)
        {
            //TextContentMove.Stop();

            //((DoubleAnimation)TextContentMoveBack.Children[0]).From = -TextContent.ActualWidth + ActualWidth;
            //TextContentMoveBack.Begin();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, e.NewSize.Width, TextContent.ActualHeight) };
        }
    }
}
