using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigDetailsViewModel : ViewModelBase
	{
		public ICommand BackCommand { get; }

		public LoadoutConfigDetailsViewModel(IAraNavigation navigations)
		{
			BackCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigs));
		}
	}
}
