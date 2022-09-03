using VtuberMusic.AppCore.Services;

namespace VtuberMusic.App.ViewModels;
public class PlayingViewModel : MusicPlayerViewModel {
    public PlayingViewModel(IMediaPlayBackService mediaPlayBackService) : base(mediaPlayBackService) {

    }
}
