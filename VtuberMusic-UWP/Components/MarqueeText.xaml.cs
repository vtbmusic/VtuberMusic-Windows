using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace VtuberMusic_UWP.Components {
    /// <summary>
    /// 滚动文本
    /// </summary>
    public partial class MarqueeText : UserControl {
        public static readonly DependencyProperty TextProperty =
       DependencyProperty.Register("Text", typeof(string), typeof(MarqueeText), new PropertyMetadata("Text", new PropertyChangedCallback(TextChangedEvntHandler)));

        private Storyboard TextContentMove => this.Resources["TextContentMove"] as Storyboard;

        private static void TextChangedEvntHandler(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( (MarqueeText)d ).TextChange(e);
        }

        private protected void TextChange(DependencyPropertyChangedEventArgs args) {
            this.TextContent.Text = (string)args.NewValue;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public MarqueeText() {
            this.InitializeComponent();
        }

        private void TextContent_PointerExited(object sender, PointerRoutedEventArgs e) {
            this.TextContentMove.Seek(this.TextContentMove.Duration.TimeSpan * 2 - this.TextContentMove.GetCurrentTime());
        }

        private void TextContent_PointerEntered(object sender, PointerRoutedEventArgs e) {
            var offest = -this.TextContent.ActualWidth + this.ActualWidth;

            if (offest < 0) {
                var current = this.TextContentMove.GetCurrentTime();

                ( (DoubleAnimation)this.TextContentMove.Children[0] ).To = offest;
                this.TextContentMove.Completed += this.TextContentMove_Completed;
                this.TextContentMove.Begin();

                if (current != null) {
                    if (current < this.TextContentMove.Duration.TimeSpan) {
                        this.TextContentMove.Seek(current);
                    } else {
                        this.TextContentMove.Seek(current - this.TextContentMove.Duration.TimeSpan);
                    }
                }
            }
        }

        private void TextContentMove_Completed(object sender, object e) {
            //TextContentMove.Stop();

            //((DoubleAnimation)TextContentMoveBack.Children[0]).From = -TextContent.ActualWidth + ActualWidth;
            //TextContentMoveBack.Begin();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.Canvas.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, e.NewSize.Width, this.TextContent.ActualHeight) };
        }
    }
}
