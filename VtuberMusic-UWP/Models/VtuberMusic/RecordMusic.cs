using PropertyChanged;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class RecordMusic {
        public int playCount { get; set; }
        public string songId { get; set; }
        public Music song { get; set; }
    }
}
