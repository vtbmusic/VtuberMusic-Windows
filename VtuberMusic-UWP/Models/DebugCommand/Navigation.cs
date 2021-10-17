using System;
using VtuberMusic_UWP.Pages;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Models.DebugCommand {
    /// <summary>
    /// 内容 Frame 下一页 Debug 命令
    /// </summary>
    public class ContentFrameGoForward : IDebugCommand {
        public string Title { get; } = "ContentFrame GoForward";
        public string Description { get; } = "内容 Frame 下一页";

        public void Do() => App.ViewModel.MainPage.ContentFrame.GoForward();
    }

    /// <summary>
    /// Root Frame 下一页 Debug 命令
    /// </summary>
    public class RootFrameGoForward : IDebugCommand {
        public string Title { get; } = "RootFrame GoForward";
        public string Description { get; } = "Root Frame 下一页";

        public void Do() => App.RootFrame.GoForward();
    }

    /// <summary>
    /// Root Frame 上一页 Debug 命令
    /// </summary>
    public class RootFrameGoBack : IDebugCommand {
        public string Title { get; } = "RootFrame GoBack";
        public string Description { get; } = "Root Frame 上一页";

        public void Do() => App.RootFrame.GoBack();
    }

    public class OpenProfile : IDebugCommand {
        public string Title { get; } = "Profile";
        public string Description { get; } = "打开指定个人资料";

        public async void Do() {
            var dialog = new ContentDialog() {
                Title = "User ID?",
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Go!",
                SecondaryButtonText = "Cancle"
            };

            var text = new TextBox() { VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top };
            dialog.Content = text;
            dialog.PrimaryButtonClick += async delegate {
                App.ViewModel.NavigateToPage(typeof(Profile), (await App.Client.GetProfile(text.Text)).Data.profile);
            };

            await dialog.ShowAsync();
        }
    }
}
