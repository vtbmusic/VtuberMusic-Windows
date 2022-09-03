using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;

namespace VtuberMusic.App.Services;
public interface INavigationService {
    Frame Frame { get; }
    bool CanGoBack { get; }
    IList<PageStackEntry> BackStack { get; }

    event NavigatedEventHandler Navigated;
    event NavigatingCancelEventHandler Navigating;
    event NavigationFailedEventHandler NavigationFailed;
    event NavigationStoppedEventHandler NavigationStopped;

    void SetContentFrame(Frame frame);
    void Navigate(Type pageType, object arg);
    void Navigate<T>(object arg = null);
    void RequestGoBack();
}
