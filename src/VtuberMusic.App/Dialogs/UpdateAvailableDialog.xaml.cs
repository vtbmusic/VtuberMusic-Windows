using Microsoft.UI.Xaml.Controls;
using VtuberMusic.AppCore.Models;
using Windows.ApplicationModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Dialogs;
public sealed partial class UpdateAvailableDialog : UserControl {
    public AppCenterReleases Releases { get; set; }

    public UpdateAvailableDialog() {
        this.InitializeComponent();

        var version = Package.Current.Id.Version;
        LocalVersionText.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
