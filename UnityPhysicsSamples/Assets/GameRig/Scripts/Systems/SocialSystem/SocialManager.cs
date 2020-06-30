using GameRig.Scripts.Systems.ScoreSystem;
using GameRig.Scripts.Utilities.Attributes;

#if UNITY_IOS
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;

#endif

namespace GameRig.Scripts.Systems.SocialSystem
{
	/// <summary>
	/// This class handles some basic social system functionality. Only works for iOS for now
	/// </summary>
	public static class SocialManager
	{
#if UNITY_IOS
		private static SocialSettings settings;
#endif

		[InitializeOnLaunch(typeof(ScoreManager))]
		private static void Initialize()
		{
#if UNITY_IOS
			LoadResources();
			Authenticate();
#endif
		}

		/// <summary>
		/// Submits the latest game score and displays native leaderboard interface 
		/// </summary>
		/// <param name="score">Score to submit, usually the game highscore</param>
		public static void ShowLeaderboards(int score)
		{
#if UNITY_IOS
			if (Social.localUser.authenticated)
			{
				SubmitScore(score);
			}
			else
			{
				Social.localUser.Authenticate((success, message) =>
				{
					if (success)
					{
						SubmitScore(score);
					}
				});
			}
#endif
		}

#if UNITY_IOS
		private static void LoadResources()
		{
			settings = Resources.Load<SocialSettings>(GameRigResourcesPaths.SocialSettings);
		}

		private static void Authenticate()
		{
			if (string.IsNullOrEmpty(settings.LeaderboardName))
			{
				return;
			}

			Social.localUser.Authenticate((success, message) =>
			{
				if (success)
				{
					SubmitScore(ScoreManager.Highscore);
				}
			});
		}

		private static void SubmitScore(int score)
		{
			Social.ReportScore(score, settings.LeaderboardName, success => { Social.ShowLeaderboardUI(); });
		}
#endif
	}
}