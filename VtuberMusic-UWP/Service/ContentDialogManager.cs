using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VtuberMusic_UWP.Models.Main;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Service {
    [AddINotifyPropertyChangedInterface]
    public class ContentDialogManager : INotifyPropertyChanged {
        private List<CancellationTokenSource> tokenSource = new List<CancellationTokenSource>();
        public int NowShowDialogIndex { get; private set; } = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<ContentDialogResult> ShowAsync(IContentDialogControl dialog) => await this.ShowAsync(dialog.ContentDialog);

        public async Task<ContentDialogResult> ShowAsync(ContentDialog dialog) {
            if (this.NowShowDialogIndex != this.tokenSource.Count) {
                try {
                    await Task.Delay(-1, tokenSource.Last().Token);
                } catch { }
            }

            tokenSource.Add(new CancellationTokenSource());

            dialog.Closed += this.Dialog_Closed;
            return await dialog.ShowAsync();
        }

        private void Dialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args) {
            tokenSource[this.NowShowDialogIndex].Cancel();
            this.NowShowDialogIndex++;
        }
    }
}