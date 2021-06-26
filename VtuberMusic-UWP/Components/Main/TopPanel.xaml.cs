using System;
using VtuberMusic_UWP.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Components.Main
{
    public sealed partial class TopPanel : UserControl
    {
        private Frame frame;

        public TopPanel()
        {
            this.InitializeComponent();
            App.ViewModel.TopPanel = this;
        }

        public void Init()
        {
            frame = App.ViewModel.MainPage.ContentFrame;

            GoBackButton.Click += GoBackButton_Click;
            GoForwardButton.Click += GoForwardButton_Click;

            frame.Navigated += ContetnFrame_Navigated;
            GoBackButton.IsEnabled = frame.CanGoBack;
            GoForwardButton.IsEnabled = frame.CanGoForward;
        }

        private void GoForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoForward) frame.GoForward();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {            
            if (frame.CanGoBack) frame.GoBack();
        }

        private void ContetnFrame_Navigated(object sender, NavigationEventArgs e)
        {
            GoBackButton.IsEnabled = App.ViewModel.MainPage.ContentFrame.CanGoBack;
            GoForwardButton.IsEnabled = App.ViewModel.MainPage.ContentFrame.CanGoForward;
        }
    }
}
