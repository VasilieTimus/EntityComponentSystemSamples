namespace GameRig.Scripts.Systems.AnalyticsSystem.Events
{
	/// <summary>
	/// This event is triggered when an endless game is finished
	/// </summary>
	public struct EndlessGameEndEvent
	{
		/// <summary>
		/// Gets or sets the score
		/// </summary>
		public int Score { get; set; }
	}
}