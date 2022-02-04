using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Components {
    public sealed partial class NavigationViewPivot : UserControl {
        public static readonly DependencyProperty PaneHeaderProperty =
            DependencyProperty.Register("PaneHeader",
                typeof(UIElement),
                typeof(NavigationViewPivot),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems",
                typeof(IList<NavigationViewPivotItem>),
                typeof(NavigationViewPivot),
                new PropertyMetadata(null));

        public IList<object> MenuItems {
            get => (IList<object>)this.GetValue(MenuItemsProperty);
            set => this.SetValue(MenuItemsProperty, value);
        }

        public UIElement PaneHeader {
            get => this.GetValue(PaneHeaderProperty) as UIElement;
            set => this.SetValue(PaneHeaderProperty, value);
        }

        public NavigationViewPivot() {
            this.MenuItems = new ObservableCollection<object>();
            this.InitializeComponent();
        }
    }

    [MarkupExtensionReturnType(ReturnType = typeof(NavigationViewPivotItem))]
    public class NavigationViewPivotItem : MarkupExtension {
        protected override object ProvideValue() => this;
        public string Title { get; set; }
        public IconElement Icon { get; set; }
        public object Content { get; set; }
    }
}
