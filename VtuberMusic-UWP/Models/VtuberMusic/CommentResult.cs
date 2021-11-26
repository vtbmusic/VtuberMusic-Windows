using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    [AddINotifyPropertyChangedInterface]
    public class CommentResult {
        public Comment[] hotComments {  get; set; }
        public Comment[] comments {  get; set; }
        public int total {  get; set; }
    }
}
