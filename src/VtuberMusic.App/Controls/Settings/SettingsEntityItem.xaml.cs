using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
