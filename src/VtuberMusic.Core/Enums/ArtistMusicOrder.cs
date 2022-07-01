using System.Runtime.Serialization;

namespace VtuberMusic.Core.Enums {
    public enum ArtistMusicOrder {
        [EnumMember(Value = "hot")]
        Hot,
        [EnumMember(Value = "time")]
        Time
    }
}
