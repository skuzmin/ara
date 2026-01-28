using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using ARA.Enums;

namespace ARA.Converters
{
    public class StatusToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is GameItemStatus status ? GetStatusImagePath(status) : GetStatusImagePath(GameItemStatus.Unknown);

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

        private static string GetStatusImagePath(GameItemStatus status) 
        {
            return $"pack://application:,,,/Assets/Icons/Statuses/{Enum.GetName(status)}.png";
		}
	}
}
