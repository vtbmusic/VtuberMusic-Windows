using System.Runtime.Serialization;

namespace VtuberMusic.Core.Enums {
    public enum ProfileGender {
        [EnumMember(Value = "0")]
        Unknow = 0,
        [EnumMember(Value = "1")]
        Woman = 1,
        [EnumMember(Value = "2")]
        Man = 2
    }
}
