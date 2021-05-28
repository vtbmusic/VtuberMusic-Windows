using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Components.Main
{
    public sealed partial class SidePanel : UserControl
    {
        public Type NowPage
        {
            get
            {
                return ContentFrame.CurrentSourcePageType;
            }
        }

        public SidePanel()
        {
            this.InitializeComponent();

            App.ViewModel.SidePanel = this;
            ContentFrame.Navigated += ContentFrame_Navigated;
            ContentFrame.Navigate(typeof(Home));
        }

        public void NavigateToPage(Type page, object args)
        {
            ContentFrame.Navigate(page, args);
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            GoForwardButton.IsEnabled = ContentFrame.CanGoForward;
            GoBackButton.IsEnabled = ContentFrame.CanGoBack;
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.GoForward();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.GoBack();
        }
    }
}
