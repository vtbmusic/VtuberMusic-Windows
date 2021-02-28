using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Components.Main;
using VtuberMusic_UWP.Components.Player;
using Windows.UI.Xaml.Media;

namespace VtuberMusic_UWP.Models.Main
{
    public class ViewModel
    {
        public MainPage MainPage;
        public SidePanel SidePanel;
        public MainPlayer MainPlayer;

        public void SetBackgroundImage(ImageSource imageSource)
        {
            MainPage.Background = new ImageBrush { ImageSource = imageSource };
        }

        public void NavigateToPage(Type page, object args = null)
        {
            SidePanel.NavigateToPage(page, args);
        }
    }
}
