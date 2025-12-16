using System.ComponentModel;
using System.Windows.Input;

namespace ARA.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public class MainViewModel : ViewModelBase
	{
		private object _currentPage;
		public object CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
			}
		}

		public ICommand ShowHomeCommand { get; }
		public ICommand ShowSettingsCommand { get; }

		public MainViewModel()
		{
			ShowHomeCommand = new RelayCommand(_ => CurrentPage = new HomeViewModel());
			ShowSettingsCommand = new RelayCommand(_ => CurrentPage = new SettingsViewModel());
			CurrentPage = new HomeViewModel();
		}
	}

	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;

		public RelayCommand(Action<object> execute) => _execute = execute;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => _execute(parameter);

		public event EventHandler CanExecuteChanged;
	}
}
