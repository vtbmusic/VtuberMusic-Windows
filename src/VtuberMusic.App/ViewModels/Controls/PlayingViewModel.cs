using VtuberMusic.AppCore.Services;

namespace VtuberMusic.App.ViewModels.Controls;
public class PlayingViewModel : MusicPlayerViewModel {
    public PlayingViewModel(IMediaPlayBackService mediaPlayBackService) : base(mediaPlayBackService) {

    }
}
