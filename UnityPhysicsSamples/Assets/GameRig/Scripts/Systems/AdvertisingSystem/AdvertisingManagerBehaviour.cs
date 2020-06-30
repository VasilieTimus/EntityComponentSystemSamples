using System;
using System.Collections;
using UnityEngine;

namespace GameRig.Scripts.Systems.AdvertisingSystem
{
	/// <summary>
	/// This class is used by <see cref="AdvertisingManager"/> for Ads load delay
	/// </summary>
	public class AdvertisingManagerBehaviour : MonoBehaviour
	{
		private Action interstitialLoad = delegate { };
		private Action rewardedAdLoad = delegate { };

		public void Initialize(Action interstitialLoadAction, Action rewardedAdLoadAction)
		{
			interstitialLoad = interstitialLoadAction;
			rewardedAdLoad = rewardedAdLoadAction;
		}

		public void LoadInterstitialWithDelay(float delay)
		{
			StartCoroutine(DelayedInterstitialLoadCoroutine(delay));
		}

		public void LoadRewardedAdWithDelay(float delay)
		{
			StartCoroutine(DelayedRewardedAdLoadCoroutine(delay));
		}

		private IEnumerator DelayedInterstitialLoadCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);

			interstitialLoad();
		}

		private IEnumerator DelayedRewardedAdLoadCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);

			rewardedAdLoad();
		}
	}
}