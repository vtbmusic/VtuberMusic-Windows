using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using VtuberMusic_UWP.Models.DebugCommand;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace VtuberMusic_UWP.Components {
    /// <summary>
    /// Debug 命令面板
    /// </summary>
    public sealed partial class DebugPanel : UserControl {
        private Popup popup = new Popup();
        private ObservableCollection<IDebugCommand> commandList => App.DebugCommandManager.CommandList;

        public DebugPanel() {
            this.InitializeComponent();
            Window.Current.SizeChanged += delegate {
                this.Height = Window.Current.Bounds.Height;
                this.Width = Window.Current.Bounds.Width;
            };

            this.popup.Child = this;
            this.ShareShadow.Receivers.Add(this.ShadowBackground);

            this.CommandList.Translation = new Vector3(0, 0, 32);
            this.InputPanel.Translation = new Vector3(0, 0, 32);
        }

        private void DebugPanel_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Escape) this.popup.IsOpen = false;
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public void Show() {
            this.Height = Window.Current.Bounds.Height;
            this.Width = Window.Current.Bounds.Width;

            this.popup.IsOpen = true;
            this.CommandInput.Focus(FocusState.Programmatic);
            InfoBarPopup.Show("Debug 命令面板仅限于调试和排障使用", "除非你知道你在做什么，否则请按 Esc 关闭面板。", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning);
        }

        private void CommandInput_KeyDown(object sender, KeyRoutedEventArgs e) {
            switch (e.Key) {
                case Windows.System.VirtualKey.Up:
                    if (this.CommandList.SelectedIndex - 1 >= 0) this.CommandList.SelectedIndex--;
                    break;
                case Windows.System.VirtualKey.Down:
                    if (this.CommandList.Items.Count > this.CommandList.SelectedIndex + 1) this.CommandList.SelectedIndex++;
                    break;
                case Windows.System.VirtualKey.Enter:
                    if (this.CommandList.SelectedItem != null) {
                        var command = (IDebugCommand)this.CommandList.SelectedItem;
                        InfoBarPopup.Show("执行命令", command.Title);
                        this.popup.IsOpen = false;

                        try {
                            command.Do();
                        } catch (Exception ex) {
                            if (ex.StackTrace == null) {
                                InfoBarPopup.Show("发生了一个异常", ex.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                            } else {
                                InfoBarPopup.Show($"发生了一个异常: { ex.Message }", ex.StackTrace, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                            }
                        }
                    }

                    break;
            }
        }

        private void CommandInput_TextChanged(object sender, TextChangedEventArgs e) {
            var result = new List<IDebugCommand>();

            foreach (var command in this.commandList) {
                if (command.Title.ToLower().IndexOf(this.CommandInput.Text.ToLower()) != -1) result.Add(command);
            }

            this.CommandList.ItemsSource = result;
            if (result.Count != 0) this.CommandList.SelectedIndex = 0;
        }

        private void CommandList_ItemClick(object sender, ItemClickEventArgs e) {
            var command = (IDebugCommand)e.ClickedItem;
            InfoBarPopup.Show("执行命令", command.Title);
            this.popup.IsOpen = false;

            command.Do();
        }
    }
}
