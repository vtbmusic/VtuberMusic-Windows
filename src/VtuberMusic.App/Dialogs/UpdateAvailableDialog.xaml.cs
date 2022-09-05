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
using VtuberMusic.AppCore.Models;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
