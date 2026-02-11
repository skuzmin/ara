using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigsViewModel : ViewModelBase
	{
		public ICommand BackCommand { get; }

		public LoadoutConfigsViewModel(IAraNavigation navigations)
		{
			BackCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.Loadout));
		}
	}
}
