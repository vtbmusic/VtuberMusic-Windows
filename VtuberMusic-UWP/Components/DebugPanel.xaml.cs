using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using VtuberMusic_UWP.Models.DebugCommand;
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
    public sealed partial class DebugPanel : UserControl
    {
        private Popup popup = new Popup();
        private ObservableCollection<IDebugCommand> commandList => App.DebugCommandManager.CommandList;

        public DebugPanel()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += delegate
            {
                this.Height = Window.Current.Bounds.Height;
                this.Width = Window.Current.Bounds.Width;
            };

            popup.Child = this;
            ShareShadow.Receivers.Add(ShadowBackground);

            CommandList.Translation = new Vector3(0, 0, 32);
            InputPanel.Translation = new Vector3(0, 0, 32);
        }

        private void DebugPanel_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape) popup.IsOpen = false;
        }

        public void Show()
        {
            this.Height = Window.Current.Bounds.Height;
            this.Width = Window.Current.Bounds.Width;

            popup.IsOpen = true;
            CommandInput.Focus(FocusState.Programmatic);
            InfoBarPopup.Show("Debug 命令面板仅限于调试和排障使用", "除非你知道你在做什么，否则请按 Esc 关闭面板。", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning);
        }

        private void CommandInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Up:
                    if (CommandList.SelectedIndex - 1 >= 0) CommandList.SelectedIndex--;
                    break;
                case Windows.System.VirtualKey.Down:
                    if (CommandList.Items.Count > CommandList.SelectedIndex + 1) CommandList.SelectedIndex++;
                    break;
                case Windows.System.VirtualKey.Enter:
                    if (CommandList.SelectedItem != null)
                    {
                        var command = (IDebugCommand)CommandList.SelectedItem;
                        InfoBarPopup.Show("执行命令", command.Title);
                        popup.IsOpen = false;

                        try
                        {
                            command.Do();
                        }
                        catch (Exception ex)
                        {
                            if (ex.StackTrace == null)
                            {
                                InfoBarPopup.Show("发生了一个异常", ex.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                            }
                            else
                            {
                                InfoBarPopup.Show($"发生了一个异常: { ex.Message }", ex.StackTrace, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                            }
                        }
                    }
                    break;
            }
        }

        private void CommandInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var result = new List<IDebugCommand>();

            foreach (var command in commandList)
            {
                if (command.Title.ToLower().IndexOf(CommandInput.Text.ToLower()) != -1) result.Add(command);
            }

            CommandList.ItemsSource = result;
            if (result.Count != 0) CommandList.SelectedIndex = 0;
        }

        private void CommandList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var command = (IDebugCommand)e.ClickedItem;
            InfoBarPopup.Show("执行命令", command.Title);
            popup.IsOpen = false;

            try
            {
                command.Do();
            }
            catch (Exception ex)
            {
                if (ex.StackTrace == null)
                {
                    InfoBarPopup.Show("发生了一个异常", ex.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                }
                else
                {
                    InfoBarPopup.Show($"发生了一个异常: { ex.Message }", ex.StackTrace, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
                }
            }
        }
    }
}
