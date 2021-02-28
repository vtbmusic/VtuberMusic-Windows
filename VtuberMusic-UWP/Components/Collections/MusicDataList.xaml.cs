using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.VtuberMusic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace VtuberMusic_UWP.Components.Collections
{
    public partial class MusicDataList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(MusicData[]), typeof(MusicDataList), new PropertyMetadata("ItemsSource", new PropertyChangedCallback(ItemsSourceChangeEventHandle)));

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(MusicDataListMode), typeof(MusicDataList), new PropertyMetadata("Mode", new PropertyChangedCallback(ModeChangeEventHandle)));

        public MusicData[] ItemsSource
        {
            get { return (MusicData[])GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public MusicDataListMode Mode
        {
            get { return (MusicDataListMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public MusicDataList()
        {
            this.InitializeComponent();
        }

        private static void ModeChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MusicDataList)d).ModeChange(e);
        }

        private static void ItemsSourceChangeEventHandle(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MusicDataList)d).ItemsSourceChange(e);
        }

        private protected void ItemsSourceChange(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.GetType() == typeof(MusicData[]))
            {
                DataList.ItemsSource = e.NewValue;
            }
        }

        private protected void ModeChange(DependencyPropertyChangedEventArgs args)
        {
            switch ((MusicDataListMode)args.NewValue)
            {
                case MusicDataListMode.Small:
                    DataList.ItemTemplate = Small;
                    break;
                case MusicDataListMode.Large:
                    DataList.ItemTemplate = Large;
                    break;
            }
        }

        private void DataList_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.ItemIndex % 2 == 0)
            {
                var brush = new SolidColorBrush((Color)Resources["SystemListLowColor"]);
                args.ItemContainer.Background = brush;
            }
        }

        private void DataList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (DataList.SelectedItem != null)
            {
                App.Player.SetMusic((MusicData)DataList.SelectedItem);
            }
        }
    }

    public enum MusicDataListMode
    {
        Small,
        Large
    }
}
