using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.App.ViewModels.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.DataItem;
public sealed partial class ArtistItem : UserControl {
    public static readonly DependencyProperty ArtistProperty =
        DependencyProperty.Register("Artist", typeof(Artist), typeof(ArtistItem), new PropertyMetadata(null));

    private readonly DataItemViewModel ViewModel = Ioc.Default.GetRequiredService<DataItemViewModel>();

    public Artist Artist {
        get => (Artist)GetValue(ArtistProperty);
        set => SetValue(ArtistProperty, value);
    }

    public ArtistItem() {
        this.InitializeComponent();
    }
}
