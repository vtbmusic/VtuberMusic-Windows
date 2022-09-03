using System;

namespace VtuberMusic.Core.Helper {
    public class DateTimeHelper {
        public static DateTimeOffset ConvertUnixTimestampToDateTimeOffset(long timestamp) => DateTimeOffset.FromUnixTimeSeconds(timestamp);

        public static long ConvertDateTimeOffsetToUnixTimestamp(DateTimeOffset datetime) => datetime.ToUnixTimeSeconds();
    }
}
