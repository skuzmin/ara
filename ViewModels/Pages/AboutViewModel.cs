namespace ARA.ViewModels.Pages
{
	public class AboutViewModel : ViewModelBase
	{
		private string _title = "About";
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}
	}
}
