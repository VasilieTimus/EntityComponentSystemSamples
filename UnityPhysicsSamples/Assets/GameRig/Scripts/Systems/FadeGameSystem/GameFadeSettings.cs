using DG.Tweening;
using UnityEngine;

namespace GameRig.Scripts.Systems.FadeGameSystem
{
	public class GameFadeSettings : ScriptableObject
	{
		[SerializeField] private bool fadeOutSceneOpen = true;
		[SerializeField] private Ease fadeEase = Ease.Linear;
		[SerializeField] private float fadeDuration = 1f;

		public bool FadeOutSceneOpen => fadeOutSceneOpen;
		public Ease FadeEase => fadeEase;
		public float FadeDuration => fadeDuration;
	}
}