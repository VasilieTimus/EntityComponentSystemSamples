using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Systems.SaveSystem;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;

namespace GameRig.Scripts.Systems.ScoreSystem
{
	/// <summary>
	/// This class handles some basic scoring system
	/// </summary>
	public static class ScoreManager
	{
		private static int score;
		private static bool wasHighscoreBeaten;
		private static float nextCustomScore;

		/// <summary>
		/// Gets the current highscore
		/// </summary>
		public static int Highscore { get; private set; }

		/// <summary>
		/// Gets or sets the current custom score step
		/// </summary>
		public static int CustomScoreStep { get; set; }

		/// <summary>
		/// Gets or sets the current game score
		/// </summary>
		public static int Score
		{
			get => score;
			set
			{
				if (score == value)
				{
					return;
				}

				int deltaScore = value - score;

				score = value;

				if (CustomScoreStep > 0 && score > nextCustomScore)
				{
					if (nextCustomScore > 0)
					{
						OnCustomScoreBeat(nextCustomScore);
					}

					nextCustomScore += CustomScoreStep;
				}

				if (score > Highscore && !wasHighscoreBeaten && Highscore > 0)
				{
					wasHighscoreBeaten = true;

					OnHighscoreBeat(score);
				}

				OnScoreChange(value, deltaScore);
			}
		}

		public delegate void CustomScoreBeatDelegate(float customScore);

		public delegate void HighscoreBeatDelegate(int highscore);

		public delegate void ScoreChangeDelegate(int score, int deltaScore);

		public static CustomScoreBeatDelegate OnCustomScoreBeat = delegate { };
		public static HighscoreBeatDelegate OnHighscoreBeat = delegate { };
		public static ScoreChangeDelegate OnScoreChange = delegate { };

		[InitializeOnLaunch(typeof(PubSubManager))]
		private static void Initialize()
		{
			Score = SaveManager.Load(GameRigSaveKeys.Score, 0);
			Highscore = SaveManager.Load(GameRigSaveKeys.Highscore, 0);
		}

		/// <summary>
		/// Saves the current score for the next game launch
		/// </summary>
		public static void Save()
		{
			SaveManager.Save(GameRigSaveKeys.Score, score);
		}

		/// <summary>
		/// Resets and saves all score related data
		/// </summary>
		public static void Reset()
		{
			if (score > Highscore)
			{
				Highscore = score;

				SaveManager.Save(GameRigSaveKeys.Highscore, Highscore);
			}

			score = 0;

			SaveManager.Save(GameRigSaveKeys.Score, score);
		}

		/// <summary>
		/// Resets next custom score step to zero
		/// </summary>
		public static void ResetCustomScore()
		{
			nextCustomScore = 0;
		}
	}
}