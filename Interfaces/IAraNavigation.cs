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
		public void NavigateToPage(AraPage page);
		public void NavigateToDefaultPage();
	}
}
