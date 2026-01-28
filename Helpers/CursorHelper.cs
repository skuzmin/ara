using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Win32.SafeHandles;

namespace ARA.Helpers
{
    public static class CursorHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo info);

        [DllImport("user32.dll")]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        public static Cursor CreateCursorFromPng(string path, int xHotspot, int yHotspot)
        {
            var streamInfo = Application.GetResourceStream(new Uri(path)) ?? throw new Exception("Resource not found: " + path);

            using var stream = streamInfo.Stream;
            var bitmap = new System.Drawing.Bitmap(stream);

            IconInfo info = new();
            GetIconInfo(bitmap.GetHicon(), ref info);
            info.xHotspot = xHotspot;
            info.yHotspot = yHotspot;
            info.fIcon = false;

            IntPtr cursrPtr = CreateIconIndirect(ref info);
            SafeFileHandle handle = new(cursrPtr, true);
            return CursorInteropHelper.Create(handle);
        }
    }
}
