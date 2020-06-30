using Bayat.SaveSystem;

namespace GameRig.Scripts.Systems.SaveSystem
{
	public static class SaveManager
	{
		public static void Save<T>(string identifier, T value)
		{
			SaveSystemAPI.SaveAsync(identifier, value);
		}

		public static T Load<T>(string identifier, T defaultValue)
		{
			return SaveSystemAPI.ExistsAsync(identifier).Result ? SaveSystemAPI.LoadAsync<T>(identifier).Result : defaultValue;
		}

		public static void Delete(string identifier)
		{
			SaveSystemAPI.DeleteAsync(identifier);
		}

		public static void DeleteAll()
		{
			SaveSystemAPI.ClearAsync();
		}

		public static bool Exists(string identifier)
		{
			return SaveSystemAPI.ExistsAsync(identifier).Result;
		}
	}
}