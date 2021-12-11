using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;

namespace VtuberMusic_UWP.Models.Main {
    public interface IContentDialogControl {
        ContentDialog ContentDialog { get; }
    }
}