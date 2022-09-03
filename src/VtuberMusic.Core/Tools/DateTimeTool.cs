using System;

namespace VtuberMusic.Core.Tools {
    public class DateTimeTool {
        public static DateTimeOffset ConvertUnixTimestampToDateTimeOffset(long timestamp) =>
            DateTimeOffset.FromUnixTimeSeconds(timestamp);

        public static long ConvertDateTimeOffsetToUnixTimestamp(DateTimeOffset datetime) =>
            datetime.ToUnixTimeSeconds();
    }
}
