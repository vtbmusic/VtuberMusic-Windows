using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using VtuberMusic.Core.Models.Lyric;

namespace VtuberMusic.App.Messages {
    public class NowLyricChangedMessage : ValueChangedMessage<NowLyricChangedMessageValue> {
        public NowLyricChangedMessage(NowLyricChangedMessageValue value) : base(value) {
        }
    }

    public class NowLyricChangedMessageValue {
        public ParsedVrc Lyric { get; set; }
        public int NowLyricIndex { get; set; } = -1;
    }
}
