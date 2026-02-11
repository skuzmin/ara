using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigsViewModel(IAraNavigation navigations) : ViewModelBase
	{
        public ICommand BackCommand { get; } = new RelayCommand(_ => navigations.NavigateToPage(AraPage.Loadout));
        public ICommand ConfigDetailsCommand { get; } = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigDetails));
    }
}
