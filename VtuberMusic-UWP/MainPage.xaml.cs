using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VtuberMusic_UWP.Pages;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace VtuberMusic_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public TypedEventHandler<object, NavigationEventArgs> Navigated;
        public Frame PageFrame => NavigationFrame;
        public Frame ContentFrame => NavigationFrame;

        public MainPage()
        {
            this.InitializeComponent();
            App.ViewModel.MainPage = this;
            App.ViewModel.TopPanel.Init();

            Navigation.SelectionChanged += Navigation_SelectionChanged;
            NavigationFrame.Navigated += NavigationFrame_Navigated;
            Navigation.ItemInvoked += Navigation_ItemInvoked;

            Navigation.SelectedItem = Navigation.MenuItems[1];
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
            if (args.IsSettingsInvoked && ContentFrame.Content.GetType() != typeof(Settings))
            {
                ContentFrame.Navigate(typeof(Settings));
                return;
            }

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
            if (e.Content is Settings)
            {
                Navigation.SelectedItem = Navigation.SettingsItem;
                return;
            }

            foreach (NavigationViewItemBase tmp in Navigation.MenuItems)
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

    [MarkupExtensionReturnType(ReturnType = typeof(NavigationItemTag))]
    public class NavigationItemTag : MarkupExtension
    {
        protected override object ProvideValue() => this;
        public Type PageType { get; set; }
        public object Args { get; set; }
    }
}
