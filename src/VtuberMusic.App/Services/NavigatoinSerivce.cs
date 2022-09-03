using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;

namespace VtuberMusic.App.Services;
public class NavigatoinSerivce : INavigationService {
    public Frame Frame { get; private set; }
    public bool CanGoBack => Frame.CanGoBack;
    public IList<PageStackEntry> BackStack => Frame.BackStack;

    public event NavigatedEventHandler Navigated;
    public event NavigatingCancelEventHandler Navigating;
    public event NavigationFailedEventHandler NavigationFailed;
    public event NavigationStoppedEventHandler NavigationStopped;

    public void RequestGoBack() => Frame.GoBack();

    public void Navigate(Type pageType, object arg) => Frame.Navigate(pageType, arg);

    public void Navigate<T>(object arg = null) => Frame.Navigate(typeof(T), arg);

    public void SetContentFrame(Frame frame) {
        Frame = frame;
        frame.Navigated += Frame_Navigated;
        frame.Navigating += Frame_Navigating;
        frame.NavigationFailed += Frame_NavigationFailed;
        frame.NavigationStopped += Frame_NavigationStopped;
    }

    private void Frame_NavigationStopped(object sender, NavigationEventArgs e) => NavigationStopped?.Invoke(this, e);

    private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(this, e);

    private void Frame_Navigating(object sender, NavigatingCancelEventArgs e) => Navigating?.Invoke(this, e);

    private void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(this, e);
}
