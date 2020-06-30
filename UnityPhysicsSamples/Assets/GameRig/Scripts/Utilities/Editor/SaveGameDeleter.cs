using GameRig.Scripts.Systems.SaveSystem;
using UnityEditor;

namespace GameRig.Scripts.Utilities.Editor
{
	/// <summary>
	/// This class is used to delete all game saves
	/// </summary>
	public class SaveGameDeleter : UnityEditor.Editor
	{
		[MenuItem("GameRig/Delete Game Save", priority = -100)]
		private static void DeleteAll()
		{
			SaveManager.DeleteAll();
		}
	}
}