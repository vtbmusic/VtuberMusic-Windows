using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VtuberMusic_UWP.Service;
using VtuberMusic_UWP.Tools;

namespace VtuberMusic_UWP.Models.Main {
    public class AppCenterReportAttachment {
        public LocalSettingsManager LocalSettings { get; set; }
        public RoamingSettingsManager RoamingSettings { get; set; }
        public string Verson { get; set; }
        public Player Player { get; set; }

        public static AppCenterReportAttachment Create() {
            return new AppCenterReportAttachment {
                LocalSettings = App.LocalSettings,
                RoamingSettings = App.RoamingSettings,
                Verson = UsefullTools.GetGitCommitInfo(),
                Player = App.Player
            };
        }

        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
