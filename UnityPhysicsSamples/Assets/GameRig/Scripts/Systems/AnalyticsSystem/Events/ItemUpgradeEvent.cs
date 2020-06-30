namespace GameRig.Scripts.Systems.AnalyticsSystem.Events
{
	public struct ItemUpgradeEvent
	{
		public string ItemName { get; set; }
		public string ItemType { get; set; }
		public string Level { get; set; }
	}
}