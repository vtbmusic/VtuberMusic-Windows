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
public sealed partial class SettingsButtonEntityItem : UserControl {
    public static DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(object), typeof(SettingsButtonEntityItem), new PropertyMetadata(null));
    public static DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(SettingsButtonEntityItem), new PropertyMetadata(null));
    public static DependencyProperty DescrptionProperty =
        DependencyProperty.Register("Descrption", typeof(string), typeof(SettingsButtonEntityItem), new PropertyMetadata(null));
    public new static DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(SettingsButtonEntityItem), new PropertyMetadata(null));

    public static DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(SettingsButtonEntityItem), new PropertyMetadata(null, OnCommandPropertyChanged));

    public static DependencyProperty CommandParameterProperty =
        DependencyProperty.Register("CommandParameter", typeof(ICommand), typeof(SettingsButtonEntityItem), new PropertyMetadata(null, OnCommandParameterPropertyChanged));

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

    public ICommand Command {
        get => GetValue(CommandProperty) as ICommand;
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public event RoutedEventHandler Clicked;

    public SettingsButtonEntityItem() {
        this.InitializeComponent();
    }

    private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsButtonEntityItem).SettingsButton.Command = e.NewValue as ICommand;
    private static void OnCommandParameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsButtonEntityItem).SettingsButton.CommandParameter = e.NewValue;

    private void SettingsButton_Click(object sender, RoutedEventArgs e) => Clicked?.Invoke(this, e);
}
