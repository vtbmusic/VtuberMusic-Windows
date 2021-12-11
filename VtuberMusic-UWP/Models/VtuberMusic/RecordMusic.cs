using PropertyChanged;
using System.ComponentModel;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class RecordMusic : INotifyPropertyChanged {
        public int playCount { get; set; }
        public string songId { get; set; }
        public Music song { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
