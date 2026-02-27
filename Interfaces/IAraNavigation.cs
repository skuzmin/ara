using System.ComponentModel;
using System.Windows.Input;
using ARA.Enums;
using ARA.ViewModels;

namespace ARA.Interfaces
{
	public interface IAraNavigation : INotifyPropertyChanged
	{
		public ViewModelBase? CurrentPage { get; set; }
		public ICommand NavigateToPageCommand { get; }
		public bool TryNavigateToPage(AraPage page);
		public void NavigateToDefaultPage();
		public bool CanNavigateToPage(AraPage page);
	}
}
