﻿using System.Runtime.InteropServices;

namespace VtuberMusic.App.Helper;
public class WindowsSystemDispatcherQueueHelper {
    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }

    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    private object m_dispatcherQueueController = null;
    public void EnsureWindowsSystemDispatcherQueueController() {
        if (Windows.System.DispatcherQueue.GetForCurrentThread() != null) {
            // one already exists, so we'll just use it.
            return;
        }

        if (m_dispatcherQueueController == null) {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

            CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
        }
    }
}