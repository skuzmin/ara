using System.Windows.Input;

namespace ARA.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		private string _title = "Home page";
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		public ICommand ChangeTitleCommand { get; }

		public HomeViewModel()
		{
			ChangeTitleCommand = new RelayCommand(_ => Title = "Home title changed");
		}
	}
}
