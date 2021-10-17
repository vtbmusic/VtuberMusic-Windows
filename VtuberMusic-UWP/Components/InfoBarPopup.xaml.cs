using Microsoft.UI.Xaml.Controls;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace VtuberMusic_UWP.Components {
    /// <summary>
    /// 消息提示气泡
    /// </summary>
    public sealed partial class InfoBarPopup : UserControl {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title {
            get { return this.Info.Title; }
            set { this.Info.Title = value; }
        }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message {
            get { return this.Info.Message; }
            set { if (value != null) this.Info.Message = value; }
        }
        /// <summary>
        /// Icon 是否可见
        /// </summary>
        public bool IsIconVisible {
            get { return this.Info.IsIconVisible; }
            set { this.Info.IsIconVisible = value; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public InfoBarSeverity Severity {
            get { return this.Info.Severity; }
            set { this.Info.Severity = value; }
        }

        public InfoBarPopup() {
            this.InitializeComponent();

            this.ShareShadow.Receivers.Add(this.ShadowBackground);
            this.Info.Translation = new Vector3(0, 0, 25);
        }

        /// <summary>
        /// 显示气泡
        /// </summary>
        public async void Show() {
            var popup = new Popup();
            popup.IsOpen = true;
            popup.Child = this;

            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;

            this.PopupOut.Completed += delegate {
                popup.IsOpen = false;
            };

            this.PopupIn.Begin();
            await Task.Delay(2000);
            this.PopupOut.Begin();
        }

        /// <summary>
        /// 显示奇葩
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="severity">等级</param>
        /// <param name="isIconVisible">Icon 是否可见</param>
        public static void Show(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, bool isIconVisible = true) {
            var popup = new InfoBarPopup();

            popup.Title = title;
            popup.Message = message;
            popup.IsIconVisible = isIconVisible;
            popup.Severity = severity;

            popup.Show();
        }
    }
}
