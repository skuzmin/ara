using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Win32.SafeHandles;
#pragma warning disable SYSLIB1054

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

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);

		[DllImport("user32.dll")]
		private static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo info);

        [DllImport("user32.dll")]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        public static Cursor CreateCursorFromPng(string path, int xHotspot, int yHotspot)
        {
            var streamInfo = Application.GetResourceStream(new Uri(path)) ?? throw new Exception("Resource not found: " + path);

            using var stream = streamInfo.Stream;
            using var bitmap = new System.Drawing.Bitmap(stream);

            IconInfo info = new();
			IntPtr iconHandle = bitmap.GetHicon();
			GetIconInfo(iconHandle, ref info);
            info.xHotspot = xHotspot;
            info.yHotspot = yHotspot;
            info.fIcon = false;
            IntPtr cursrPtr = CreateIconIndirect(ref info);

			DestroyIcon(iconHandle);
			DeleteObject(info.hbmMask);
			DeleteObject(info.hbmColor);

			SafeFileHandle handle = new(cursrPtr, true);
            return CursorInteropHelper.Create(handle);
        }
    }
}
