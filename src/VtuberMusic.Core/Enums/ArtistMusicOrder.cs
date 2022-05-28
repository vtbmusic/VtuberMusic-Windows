using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace VtuberMusic.Core.Enums {
    public enum ArtistMusicOrder {
        [EnumMember(Value = "hot")]
        Hot,
        [EnumMember(Value = "time")]
        Time
    }
}
