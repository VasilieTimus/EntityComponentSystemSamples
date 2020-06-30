namespace GameRig.Scripts.Systems.AnalyticsSystem.Events
{
	/// <summary>
	/// This event is triggered when the game/level is started
	/// </summary>
	public struct LevelStartEvent
	{
		/// <summary>
		/// Gets or sets the level started value
		/// </summary>
		public int Level { get; set; }
	}
}