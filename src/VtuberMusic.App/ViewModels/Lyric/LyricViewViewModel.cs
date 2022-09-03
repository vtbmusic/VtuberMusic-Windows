using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Helper;
using VtuberMusic.Core.Models.Lyric;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Lyric;
public partial class LyricViewViewModel : ObservableRecipient {
    private readonly IVtuberMusicService _vtuberMusicService;
    private readonly IMediaPlayBackService _mediaPlayBackService;

    [ObservableProperty]
    private ParsedVrc lyric;

    public LyricViewViewModel(IVtuberMusicService vtuberMusicService, IMediaPlayBackService mediaPlayBackService) {
        _vtuberMusicService = vtuberMusicService;
        _mediaPlayBackService = mediaPlayBackService;
    }

    protected override void OnActivated() {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackMusicChangedMessage message) {
            DispatcherHelper.TryRun(async delegate {
                await Load();
            });
        });
    }

    protected override void OnDeactivated() {
        base.OnDeactivated();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    [RelayCommand]
    private async Task Load() {
        if (_mediaPlayBackService.NowPlaying != null) {
            try {
                this.Lyric = await LyricHelper.ParsedVrcAsync(await _vtuberMusicService.GetLyric(_mediaPlayBackService.NowPlaying.id));
            } catch {
                this.Lyric = null;
            }
        }
    }
}
