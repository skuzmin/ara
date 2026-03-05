using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using ARA.Interfaces;
#pragma warning disable SYSLIB1054

namespace ARA.Services
{
	public class GlobalHotKeyService : IDisposable
	{
		private const int WM_HOTKEY = 0x0312;

		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		private const uint MOD_CONTROL = 0x0002;
		private const uint MOD_ALT = 0x0001;
		private const uint VK_O = 0x4F;
		private const uint VK_E = 0x45;

		private const int HOTKEY_SHOW = 1;
		private const int HOTKEY_ACTION = 2;

		private readonly IMainWindow _mainWindowService;
		private HwndSource? _source;
		private IntPtr _handle;

		public GlobalHotKeyService(IMainWindow mainWindowService)
		{
			_mainWindowService = mainWindowService;
		}

		public void Register(Window window)
		{
			_handle = new WindowInteropHelper(window).Handle;
			_source = HwndSource.FromHwnd(_handle);
			_source.AddHook(HwndHook);

			RegisterHotKey(_handle, HOTKEY_SHOW, MOD_CONTROL | MOD_ALT, VK_O);
			RegisterHotKey(_handle, HOTKEY_ACTION, MOD_CONTROL | MOD_ALT, VK_E);
		}

		private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_HOTKEY)
			{
				switch (wParam.ToInt32())
				{
					case HOTKEY_SHOW:
						_mainWindowService.ShowMainWindow();
						handled = true;
						break;
					case HOTKEY_ACTION:
						handled = true;
						break;
				}
			}
			return IntPtr.Zero;
		}

		public void Dispose()
		{
			UnregisterHotKey(_handle, HOTKEY_SHOW);
			UnregisterHotKey(_handle, HOTKEY_ACTION);
			_source?.RemoveHook(HwndHook);
			GC.SuppressFinalize(this);
		}
	}
}
