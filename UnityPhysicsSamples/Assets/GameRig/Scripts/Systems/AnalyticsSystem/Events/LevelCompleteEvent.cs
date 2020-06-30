namespace GameRig.Scripts.Systems.AnalyticsSystem.Events
{
	/// <summary>
	/// This event is triggered when the level is completed with given result
	/// </summary>
	public struct LevelCompleteEvent
	{
		/// <summary>
		/// True if the game or level was completed successfully, false otherwise
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Gets or sets the completed level index
		/// </summary>
		public int Level { get; set; }
	}
}