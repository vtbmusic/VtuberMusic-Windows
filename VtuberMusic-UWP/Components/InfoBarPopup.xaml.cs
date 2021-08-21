using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace VtuberMusic_UWP.Components
{
    public sealed partial class InfoBarPopup : UserControl
    {
        public string Title
        {
            get { return Info.Title; }
            set { Info.Title = value; }
        }
        public string Message
        {
            get { return Info.Message; }
            set { Info.Message = value; }
        }
        public bool IsIconVisible
        {
            get { return Info.IsIconVisible; }
            set { Info.IsIconVisible = value; }
        }
        public InfoBarSeverity Severity
        {
            get { return Info.Severity; }
            set { Info.Severity = value; }
        }

        public InfoBarPopup()
        {
            this.InitializeComponent();

            ShareShadow.Receivers.Add(ShadowBackground);
            Info.Translation = new Vector3(0, 0, 25);
        }

        public async void Show()
        {
            var popup = new Popup();
            popup.IsOpen = true;
            popup.Child = this;

            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;

            PopupOut.Completed += delegate
            {
                popup.IsOpen = false;
            };

            PopupIn.Begin();
            await Task.Delay(2000);
            PopupOut.Begin();
        }

        public static void Show(string title, string message, bool isIconVisible = true, InfoBarSeverity severity = InfoBarSeverity.Informational)
        {
            var popup = new InfoBarPopup();
            
            popup.Title = title;
            popup.Message = message;
            popup.IsIconVisible = isIconVisible;
            popup.Severity = severity;

            popup.Show();
        }
    }
}
