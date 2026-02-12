using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;
using Microsoft.Extensions.Logging;

namespace ARA.ViewModels.Pages
{
	public class LoadoutConfigsViewModel : ViewModelBase
	{
		private readonly ILogger _logger;
		public List<LoadoutConfiguration> LoadoutConfigurations { get; set; }
		public ICommand BackCommand { get; }
		public ICommand ConfigDetailsCommand { get; }

		public LoadoutConfigsViewModel(IAraNavigation navigations, ILogger logger, IAraConfigurations configurations)
		{
			_logger = logger;
			LoadoutConfigurations = configurations.Configurations.LoadoutConfigurations;
			BackCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.Loadout));
			ConfigDetailsCommand = new RelayCommand(_ => navigations.NavigateToPage(AraPage.LoadoutConfigDetails));
		}
	}
}
