using System.Collections.ObjectModel;
using System.Windows.Input;
using ARA.Enums;
using ARA.Interfaces;
using ARA.Models;

namespace ARA.ViewModels.Pages
{
	public class LoadoutViewModel : ViewModelBase
	{
		public ICommand CheckLoadout { get; }
		public ObservableCollection<LoadoutConfiguration> LoadoutOptions { get; }
		public LoadoutConfiguration? SelectedLoadout
		{
			get => field;
			set
			{
				value?.Items.ForEach(x => x.Status = 0);
				field = value;
				OnPropertyChanged(nameof(SelectedLoadout));
			}
		}

		public LoadoutViewModel(IConfigurations config)
		{
			SelectedLoadout = null;
			CheckLoadout = new RelayCommand(OnCheckLoadoutClicked);
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
		}
        private void OnCheckLoadoutClicked(object obj)
        {
			if (SelectedLoadout == null)
			{
				return;
			}

			var newItems = SelectedLoadout.Items.Select(item => new GameItem
			{
				Icon = item.Icon,
				Quantity = item.Quantity,
				Status = (GameItemStatus)new Random().Next(1, 3)
			})
			.OrderByDescending(item => item.Status)
			.ToList();

			SelectedLoadout.Items = newItems;
			OnPropertyChanged(nameof(SelectedLoadout));
		}
    }
}
