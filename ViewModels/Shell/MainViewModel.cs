using ARA.Interfaces;

namespace ARA.ViewModels.Shell
{
	public class MainViewModel : ViewModelBase
	{
		public IAraNavigation Navigation { get; }
		public MainViewModel(IAraNavigation navigation)
		{
			Navigation = navigation;
			Navigation.NavigateToDefaultPage();
		}
	}
}
