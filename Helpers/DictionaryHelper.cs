using System.Windows;

namespace ARA.Helpers
{
    public static class DictionaryHelper
    {
        public static void UpdateMergedDictionary(Uri uri, string key)
        {
			var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
			var dictionary = mergedDictionaries.FirstOrDefault(d =>
				d.Contains("__Title") && d["__Title"] as string == key);

			if (dictionary != null)
			{
				mergedDictionaries.Remove(dictionary);
			}

			mergedDictionaries.Add(new ResourceDictionary { Source = uri });
		}
    }
}
