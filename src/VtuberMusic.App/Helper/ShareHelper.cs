using System;
using VtuberMusic.AppCore.Helper;
using VtuberMusic.Core.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace VtuberMusic.App.Helper;
public class ShareHelper {
    public static void ShowShareUI(Action<DataTransferManager, DataRequestedEventArgs> action) {
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        var interop = DataTransferManager.As<IDataTransferManagerInterop>();
        var guid = Guid.Parse("a5caee9b-8708-49d1-8d36-67d25a8da00c");
        var iop = DataTransferManager.As<IDataTransferManagerInterop>();
        var dataTransferManager = DataTransferManager.FromAbi(iop.GetForWindow(windowHandle, guid));

        dataTransferManager.DataRequested += (DataTransferManager sender, DataRequestedEventArgs arg) => action(sender, arg);
        interop.ShowShareUIForWindow(windowHandle);
    }

    public static void ShareMusic(Music music) {
        ShowShareUI(delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
            var request = arg.Request;
            request.Data.SetWebLink(new Uri($"https://vtbmusic.com/song?id={music.id}"));
            request.Data.Properties.ApplicationName = "VtuberMusic";
            request.Data.Properties.Title = music.name;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(music.picUrl));
            request.Data.Properties.Description = $"{music.name} - {MusicHelepr.GetArtistString(music.artists)}";
        });
    }

    public static void SharePlaylist(Playlist playlist) {
        ShowShareUI(delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
            var request = arg.Request;
            request.Data.SetWebLink(new Uri($"https://vtbmusic.com/songlist?id={playlist.id}"));
            request.Data.Properties.ApplicationName = "VtuberMusic";
            request.Data.Properties.Title = playlist.name;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(playlist.coverImgUrl));
            request.Data.Properties.Description = $"{playlist.name} - {playlist.creator.nickname}";
        });
    }

    public static void ShareArtist(Artist artist) {
        ShowShareUI(delegate (DataTransferManager sender, DataRequestedEventArgs arg) {
            var request = arg.Request;
            request.Data.SetWebLink(new Uri($"https://vtbmusic.com/vtuber?id={artist.id}"));
            request.Data.Properties.ApplicationName = "VtuberMusic";
            request.Data.Properties.Title = artist.name.origin;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(artist.imgUrl));
            request.Data.Properties.Description = $"{artist.name.origin} - {artist.groupName}";
        });
    }

    [System.Runtime.InteropServices.ComImport, System.Runtime.InteropServices.Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
    [System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
    interface IDataTransferManagerInterop {
        IntPtr GetForWindow([System.Runtime.InteropServices.In] IntPtr appWindow, [System.Runtime.InteropServices.In] ref Guid riid);
        void ShowShareUIForWindow(IntPtr appWindow);
    }
}
