using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigDetailsViewModel(IAraNavigation navigations) : ViewModelBase
	{
        public ICommand BackCommand { get; } = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigs));
    }
}
