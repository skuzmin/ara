using System.Collections.ObjectModel;
using ARA.Enums;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class LoadoutViewModel : ViewModelBase
	{
		public ObservableCollection<LoadoutConfiguration> LoadoutOptions { get; }

		private LoadoutConfiguration _selectedLoadout;
		public LoadoutConfiguration SelectedLoadout
		{
			get => _selectedLoadout;
			set
			{
				_selectedLoadout = value;
				OnPropertyChanged(nameof(SelectedLoadout));
			}
		}

		public LoadoutViewModel()
		{
			LoadoutOptions = [
				new LoadoutConfiguration { Name = "Dam Night Juice", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 5 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 5 },
						new GameItem{Icon = GameIcon.Door_Blocker, Quantity = 3 },
						new GameItem{Icon = GameIcon.Heavy_Fuse_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Adrenaline_Shot, Quantity = 3 },
					]
				},
				new LoadoutConfiguration { Name = "Blue Gate", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 5 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 5 },
						new GameItem{Icon = GameIcon.Barricade_Kit, Quantity = 3 },
						new GameItem{Icon = GameIcon.Lil_Smoke_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Adrenaline_Shot, Quantity = 3 },
					]
				},
				new LoadoutConfiguration { Name = "Stela Montis Poor", Items = [
						new GameItem{Icon = GameIcon.Bandage, Quantity = 3 },
						new GameItem{Icon = GameIcon.Shield_Recharger, Quantity = 3 },
						new GameItem{Icon = GameIcon.Lil_Smoke_Grenade, Quantity = 2 },
						new GameItem{Icon = GameIcon.Raider_Hatch_Key, Quantity = 1 },
					]
				}
			];
			_selectedLoadout = LoadoutOptions[0];
		}
	}
}
