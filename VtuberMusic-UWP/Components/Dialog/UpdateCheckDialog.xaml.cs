using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.Main;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Components.Dialog {
    public sealed partial class UpdateCheckDialog : ContentDialog {
        private UpdateCheck info;

        public UpdateCheckDialog(UpdateCheck updateInfo) {
            this.info = updateInfo;
            this.InitializeComponent();
            this.Title = $"发现新版本 - v{ info.version }";
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            await Launcher.LaunchUriAsync(new Uri(info.url));
#if !DEBUG
            Environment.Exit(0);
#endif
        }
    }
}
