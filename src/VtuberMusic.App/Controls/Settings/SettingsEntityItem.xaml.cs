using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.Settings;
public sealed partial class SettingsEntityItem : UserControl {
    public static DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(object), typeof(SettingsEntityItem), new PropertyMetadata(null));
    public static DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(SettingsEntityItem), new PropertyMetadata(null));
    public static DependencyProperty DescrptionProperty =
        DependencyProperty.Register("Descrption", typeof(string), typeof(SettingsEntityItem), new PropertyMetadata(null));
    public new static DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(SettingsEntityItem), new PropertyMetadata(null));

    public object Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Title {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Descrption {
        get => (string)GetValue(DescrptionProperty);
        set => SetValue(DescrptionProperty, value);
    }

    public new object Content {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public SettingsEntityItem() {
        this.InitializeComponent();
    }
}
