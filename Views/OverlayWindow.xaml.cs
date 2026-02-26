using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ARA.Enums;
using ARA.Models;

namespace ARA.Views
{
	public partial class OverlayWindow : Window
	{
		public event Action<ScreenCoordinates>? OnSave;
		private const double MinSize = 20d;
		private const double HandleSize = 12d;
		private double _x, _y, _width, _height;
		private Point _dragStart;
		private DragMode _activeHandle = DragMode.None;
		public OverlayWindow(ScreenCoordinates coordinates)
		{
			InitializeComponent();
			_x = coordinates.X;
			_y = coordinates.Y;
			_width = coordinates.Width;
			_height = coordinates.Height;

			KeyDown += OnKeyDown;
			Loaded += OnLoaded;

			OverlayCanvas.MouseDown += OverlayCanvas_MouseDown;
			OverlayCanvas.MouseMove += OverlayCanvas_MouseMove;
			OverlayCanvas.MouseUp += OverlayCanvas_MouseUp;

			SelectionBorder.Cursor = GetCursor(DragMode.Move);
			Cursor = App.AppCursor;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Focus();
			BuildHandles();
			UpdatePositions();
		}

		private void BuildHandles()
		{
			foreach (DragMode mode in Enum.GetValues<DragMode>())
			{
				if (mode == DragMode.None || mode == DragMode.Move)
				{
					continue;
				}

				var rect = new Rectangle
				{
					Width = HandleSize,
					Height = HandleSize,
					Fill = Brushes.White,
					Stroke = Brushes.DodgerBlue,
					StrokeThickness = 1,
					Cursor = GetCursor(mode),
					Tag = mode,
				};

				rect.MouseDown += Handle_MouseDown;
				rect.MouseUp += Handle_MouseUp;
				rect.MouseMove += Handle_MouseMove;

				OverlayCanvas.Children.Add(rect);
			}
		}

		private void UpdatePositions()
		{
			Canvas.SetLeft(SelectionBorder, _x);
			Canvas.SetTop(SelectionBorder, _y);
			SelectionBorder.Width = _width;
			SelectionBorder.Height = _height;

			Canvas.SetLeft(LabelAnchor, _x);
			Canvas.SetTop(LabelAnchor, _y + _height + 8);
			LabelAnchor.Width = _width;

			var positions = GetHandlePositions();
			foreach (var handle in OverlayCanvas.Children.OfType<Rectangle>())
			{
				if (handle.Tag is DragMode mode && positions.TryGetValue(mode, out var pt))
				{
					Canvas.SetLeft(handle, pt.X - handle.Width / 2);
					Canvas.SetTop(handle, pt.Y - handle.Height / 2);
				}
			}
		}

		private Dictionary<DragMode, Point> GetHandlePositions() => new()
		{
			[DragMode.ResizeNW] = new Point(_x, _y),
			[DragMode.ResizeN] = new Point(_x + _width / 2, _y),
			[DragMode.ResizeNE] = new Point(_x + _width, _y),
			[DragMode.ResizeW] = new Point(_x, _y + _height / 2),
			[DragMode.ResizeE] = new Point(_x + _width, _y + _height / 2),
			[DragMode.ResizeSW] = new Point(_x, _y + _height),
			[DragMode.ResizeS] = new Point(_x + _width / 2, _y + _height),
			[DragMode.ResizeSE] = new Point(_x + _width, _y + _height),
		};

		private void Handle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is Rectangle rect && rect.Tag is DragMode mode)
			{
				_activeHandle = mode;
				_dragStart = e.GetPosition(OverlayCanvas);
				rect.CaptureMouse();
				e.Handled = true;
			}
		}

		private void Handle_MouseMove(object sender, MouseEventArgs e)
		{
			if (_activeHandle == DragMode.None || e.LeftButton != MouseButtonState.Pressed) return;

			var pos = e.GetPosition(OverlayCanvas);
			var dx = pos.X - _dragStart.X;
			var dy = pos.Y - _dragStart.Y;

			ApplyResize(_activeHandle, dx, dy);
			UpdatePositions();
			e.Handled = true;
			_dragStart = pos;
		}

		private void Handle_MouseUp(object sender, MouseButtonEventArgs e)
		{
			_activeHandle = DragMode.None;
			(sender as Rectangle)?.ReleaseMouseCapture();
		}

		private void ApplyResize(DragMode mode, double dx, double dy)
		{
			switch (mode)
			{
				case DragMode.Move:
					_x += dx;
					_y += dy;
					break;

				case DragMode.ResizeE:
					_width = Math.Max(MinSize, _width + dx);
					break;

				case DragMode.ResizeW:
					_width = Math.Max(MinSize, _width - dx);
					if (_width != MinSize)
					{
						_x += dx;
					}
					break;

				case DragMode.ResizeS:
					_height = Math.Max(MinSize, _height + dy);
					break;

				case DragMode.ResizeN:
					_height = Math.Max(MinSize, _height - dy);
					if (_height != MinSize)
					{
						_y += dy;
					}
					break;

				case DragMode.ResizeSE:
					_width = Math.Max(MinSize, _width + dx);
					_height = Math.Max(MinSize, _height + dy);
					break;

				case DragMode.ResizeSW:
					_width = Math.Max(MinSize, _width - dx);
					_height = Math.Max(MinSize, _height + dy);
					if (_width != MinSize)
					{
						_x += dx;
					}
					break;

				case DragMode.ResizeNE:
					_width = Math.Max(MinSize, _width + dx);
					_height = Math.Max(MinSize, _height - dy);
					if (_height != MinSize)
					{
						_y += dy;
					}
					break;

				case DragMode.ResizeNW:
					_width = Math.Max(MinSize, _width - dx);
					_height = Math.Max(MinSize, _height - dy);
					if (_width != MinSize)
					{
						_x += dx;
					}
					if (_height != MinSize)
					{
						_y += dy;
					}
					break;
			}
		}

		private static Cursor GetCursor(DragMode mode) => mode switch
		{
			DragMode.ResizeNW or DragMode.ResizeSE => Cursors.SizeNWSE,
			DragMode.ResizeNE or DragMode.ResizeSW => Cursors.SizeNESW,
			DragMode.ResizeN or DragMode.ResizeS => Cursors.SizeNS,
			DragMode.ResizeW or DragMode.ResizeE => Cursors.SizeWE,
			DragMode.Move => Cursors.SizeAll,
			_ => Cursors.Arrow,
		};

		private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				_activeHandle = DragMode.Move;
				_dragStart = e.GetPosition(OverlayCanvas);
				e.Handled = true;
			}
		}

		private void OverlayCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (_activeHandle == DragMode.None || e.LeftButton != MouseButtonState.Pressed)
			{
				return;
			}

			var pos = e.GetPosition(OverlayCanvas);
			var dx = pos.X - _dragStart.X;
			var dy = pos.Y - _dragStart.Y;

			ApplyResize(_activeHandle, dx, dy);
			UpdatePositions();
			e.Handled = true;
			_dragStart = pos;
		}

		private void OverlayCanvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			_activeHandle = DragMode.None;
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				OnSave?.Invoke(new ScreenCoordinates(_x, _y, _height, _width));
				Close();
			}

			if (e.Key == Key.Escape)
			{
				Close();
			}
		}
	}
}
