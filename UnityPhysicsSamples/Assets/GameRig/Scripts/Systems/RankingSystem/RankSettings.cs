using UnityEngine;

namespace GameRig.Scripts.Systems.RankingSystem
{
	/// <summary>
	/// This settings are used to define a rank
	/// </summary>
	[CreateAssetMenu(menuName = "GameRig/Ranking System/Rank Settings")]
	public class RankSettings : ScriptableObject
	{
		[SerializeField] [Tooltip("Rank name")]
		private string rankName;

		[SerializeField] [Tooltip("Rank sprite used to display current rank")]
		private Sprite rankSprite;

		[SerializeField] [Tooltip("How many rank points are needed to achieve this rank")]
		private int targetPoints;

		/// <summary>
		/// Rank name
		/// </summary>
		public string RankName
		{
			get => rankName;
			set => rankName = value;
		}

		/// <summary>
		/// Rank sprite used to display current rank
		/// </summary>
		public Sprite RankSprite
		{
			get => rankSprite;
			set => rankSprite = value;
		}

		/// <summary>
		/// How many rank points are needed to achieve this rank
		/// </summary>
		public int TargetPoints
		{
			get => targetPoints;
			set => targetPoints = value;
		}
	}
}