namespace ARA.Models
{
	public class LoadoutConfigurationValidation
	{
		public bool IsNameNotValid { get; set; }
		public bool IsCoordinateXNotValid { get; set; }
		public bool IsCoordinateYNotValid { get; set; }
		public bool IsCoordinateHeightNotValid { get; set; }
		public bool IsCoordinateWidthNotValid { get; set; }
		public bool IsItemsListNotValid { get; set; }
		public bool IsValidated { get; set; }
		public bool IsValid
		{
			get
			{
				return !IsNameNotValid &&
				!IsCoordinateXNotValid &&
				!IsCoordinateYNotValid &&
				!IsCoordinateHeightNotValid &&
				!IsCoordinateWidthNotValid &&
				!IsItemsListNotValid;
			}
		}
	}
}
