using System.IO;

namespace ARA.Helpers
{
	public static class FilesHelper
	{
		public static void FilesDirectoryInit()
		{
			string? directory = Path.GetDirectoryName(Constants.ConfigFilePath);
			if (!string.IsNullOrEmpty(directory))
			{
				Directory.CreateDirectory(directory);
			}
			else
			{
				throw new ArgumentException("Invalid config path!");
			}
		}
	}
}
