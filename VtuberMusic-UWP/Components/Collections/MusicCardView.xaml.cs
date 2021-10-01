using System;
using System.Numerics;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace VtuberMusic_UWP.Components.Collections {
    /// <summary>
    /// 歌曲卡片列表
    /// </summary>
    public partial class MusicCardView : UserControl {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(MusicDataList), new PropertyMetadata("ItemsSource", new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        /// <summary>
        /// 数据源
        /// </summary>
        public object ItemsSource {
            get { return this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ( (MusicCardView)d ).ItemsSourceChange(e);
        }

        private protected void ItemsSourceChange(DependencyPropertyChangedEventArgs e) {
            if (e.NewValue.GetType() == typeof(Music[])) this.DataView.ItemTemplate = Normal;
            if (e.NewValue.GetType() == typeof(RecordMusic[])) this.DataView.ItemTemplate = Record;

            this.DataView.ItemsSource = e.NewValue;
        }

        public MusicCardView() {
            this.InitializeComponent();
        }

        private void GridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) {
            args.ItemContainer.Loaded += this.ItemContainer_Loaded;
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e) {
            var itemsPanel = (ItemsStackPanel)this.DataView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;

            var itemIndex = this.DataView.IndexFromContainer(itemContainer);

            var relativeIndex = itemIndex - itemsPanel.FirstVisibleIndex;

            var uc = itemContainer.ContentTemplateRoot as Grid;

            if (itemIndex != -1 && itemIndex >= 0 && itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex) {
                var itemVisual = ElementCompositionPreview.GetElementVisual(uc);
                ElementCompositionPreview.SetIsTranslationEnabled(uc, true);

                var easingFunction = Window.Current.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

                var offsetAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertKeyFrame(0f, 100);
                offsetAnimation.InsertKeyFrame(1f, 0, easingFunction);
                offsetAnimation.Target = "Translation.X";
                offsetAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                var fadeAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertExpressionKeyFrame(0f, "0");
                fadeAnimation.InsertExpressionKeyFrame(1f, "1");
                fadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                itemVisual.StartAnimation("Translation.X", offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }

            itemContainer.Loaded -= this.ItemContainer_Loaded;
        }

        private void DataView_ItemClick(object sender, ItemClickEventArgs e) {
            if (e.ClickedItem.GetType() == typeof(RecordMusic)) {
                App.Player.SetMusic(((RecordMusic)e.ClickedItem).song);
                return;
            }

            App.Player.SetMusic((Music)e.ClickedItem);
        }
    }
}
