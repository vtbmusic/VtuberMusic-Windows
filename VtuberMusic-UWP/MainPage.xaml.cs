using Windows.UI.Xaml.Controls;
using VtuberMusic_UWP.Pages;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VtuberMusic_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public TypedEventHandler<object, NavigationEventArgs> Navigated;
        public Frame ContentFrame => NavigationFrame;
        private ObservableCollection<NavigationViewItemBase> navigationItemList = new ObservableCollection<NavigationViewItemBase>()
        {
            new NavigationViewItemHeader { Content = "发现" },
            new NavigationViewItem { Icon = new SymbolIcon(Symbol.Home), Content = "首页", Tag = new NavigationItemTag { PageType = typeof(Home) } },
            new NavigationViewItem { Icon = new FontIcon { Glyph = "\uEC44", FontFamily = new FontFamily("Segoe MDL2 Assets") }, Content = "电台", Tag = new NavigationItemTag { PageType = typeof(Radio) } }
        };

        public MainPage()
        {
            this.InitializeComponent();
            App.ViewModel.MainPage = this;
            App.ViewModel.TopPanel.Init();

            Navigation.SelectionChanged += Navigation_SelectionChanged;
            NavigationFrame.Navigated += NavigationFrame_Navigated;
            Navigation.ItemInvoked += Navigation_ItemInvoked;

            Navigation.SelectedItem = navigationItemList[1];
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = (NavigationViewItemBase)args.SelectedItem;
            if (item != null && item.Tag != null && item.Tag is NavigationItemTag)
            {
                var tag = (NavigationItemTag)item.Tag;

                if (ContentFrame.Content == null) ContentFrame.Navigate(tag.PageType);
                if (tag.PageType != ContentFrame.Content.GetType()) ContentFrame.Navigate(tag.PageType);
            }
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer;
            if (item.Tag is NavigationItemTag)
            {
                var tag = (NavigationItemTag)item.Tag;

                if (ContentFrame.Content == null) ContentFrame.Navigate(tag.PageType);
                if (tag.PageType != ContentFrame.Content.GetType()) ContentFrame.Navigate(tag.PageType);
            }
        }

        private void NavigationFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (var tmp in navigationItemList)
            {
                if (tmp.Tag != null && ((NavigationItemTag)tmp.Tag).PageType == e.Content.GetType())
                {
                    Navigation.SelectedItem = tmp;
                    return;
                }
            }

            Navigation.SelectedItem = null;
        }
    }

    public class NavigationItemTag
    {
        public Type PageType { get; set; }
        public object Args { get; set; }
    }
}
