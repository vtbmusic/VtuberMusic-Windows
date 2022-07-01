using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using VtuberMusic.App.Helper;
using VtuberMusic.AppCore.Messages;
using VtuberMusic.AppCore.Services;
using VtuberMusic.Core.Helper;
using VtuberMusic.Core.Models.Lyric;
using VtuberMusic.Core.Services;

namespace VtuberMusic.App.ViewModels.Lyric {
    public class LyricViewViewModel : AppViewModel {
        IVtuberMusicService VtuberMusicService = Ioc.Default.GetService<IVtuberMusicService>();
        IMediaPlayBackService MediaPlayBackService = Ioc.Default.GetService<IMediaPlayBackService>();

        public IAsyncRelayCommand LoadCommand { get; }

        public ParsedVrc Lyric { get => lyric; set => SetProperty(ref lyric, value); }
        private ParsedVrc lyric;

        public LyricViewViewModel() : base() {
            LoadCommand = new AsyncRelayCommand(load);

            WeakReferenceMessenger.Default.Register(this, delegate (object sender, PlaybackMusicChangedMessage message) {
                DispatcherHelper.TryRun(delegate {
                    LoadCommand.Execute(null);
                });
            });
        }

        private async Task load() {
            if (MediaPlayBackService.NowPlaying != null) {
                try {
                    Lyric = await LyricHelper.ParsedVrcAsync(await VtuberMusicService.GetLyric(MediaPlayBackService.NowPlaying.id));
                } catch {
                    Lyric = null;
                }
            }
        }
    }
}
