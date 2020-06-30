namespace GameRig.Scripts.Systems.GameStateSystem
{
	/// <summary>
	/// This enum contains the most common game states for casual games
	/// </summary>
	/// <remarks>
	/// You can expand this list to suit your needs
	/// </remarks>
	public enum GameState
	{
		None,
		Menu,
		PreGame,
		Game,
		LevelComplete,
		LevelFailed
	}
}