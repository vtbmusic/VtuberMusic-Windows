using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic.App.ViewModels.SearchPanel {
    public class SearchPanelViewModel : AppViewModel {
        public string Keyword { get => keyword; set => SetProperty(ref keyword, value); }
        private string keyword;

        public IAsyncRelayCommand SearchCommand { get; }

        public SearchPanelViewModel() {
            SearchCommand = new AsyncRelayCommand(LoadResultAsync);
        }

        protected virtual Task LoadResultAsync() { return Task.CompletedTask; }
    }
}
