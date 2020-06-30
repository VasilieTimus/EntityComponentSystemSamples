using DG.Tweening;
using GameRig.Scripts.Systems.PubSubSystem;
using GameRig.Scripts.Utilities;
using GameRig.Scripts.Utilities.Attributes;
using GameRig.Scripts.Utilities.GameRigConstantValues;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameRig.Scripts.Systems.FadeGameSystem
{
	public static class FadeGameManager
	{
		private static Image fadeImage;
		private static Ease fadeEase;
		private static float fadeDurationAnim;
		private static bool fadeOutSceneOpen;

		private static GameFadeSettings settings;

		public delegate void FadeCompleteDelegate(FadeType fadeType);

		public static FadeCompleteDelegate OnFadeComplete = delegate { };

		[InitializeOnLaunch(typeof(PubSubManager))]
		public static void Initialize()
		{
			InitData();
		}

		private static void InitData()
		{
			SceneManager.sceneLoaded += SceneLoaded;

			settings = Resources.Load<GameFadeSettings>(GameRigResourcesPaths.GameFadeSettings);

			fadeOutSceneOpen = settings.FadeOutSceneOpen;
			fadeDurationAnim = settings.FadeDuration;
			fadeEase = settings.FadeEase;
		}

		private static void SceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			InstantiateFadeImage();
		}

		private static void InstantiateFadeImage()
		{
			Canvas canvasObject = Object.FindObjectOfType<Canvas>();

			if (canvasObject == null)
			{
				return;
			}

			Transform canvas = canvasObject.transform;

			if (fadeImage != null)
			{
				Object.Destroy(fadeImage.gameObject);
			}
			
			fadeImage = Object.Instantiate(new GameObject(), canvas).AddComponent<Image>().GetComponent<Image>();
			fadeImage.color = Color.black;

			RectTransform fadeImageRectTransform = fadeImage.GetComponent<RectTransform>();

			fadeImageRectTransform.anchorMin = Vector2.zero;
			fadeImageRectTransform.anchorMax = Vector2.one;

			DisableFadeImage();

			if (fadeOutSceneOpen)
			{
				StartGameFade(FadeType.FadeOut);
			}
		}

		private static void DisableFadeImage()
		{
			fadeImage.SetTransparency(0);
			fadeImage.gameObject.SetActive(false);
		}

		public static void StartGameFade(FadeType fadeType)
		{
			fadeImage.gameObject.SetActive(true);

			switch (fadeType)
			{
				case FadeType.FadeIn:
					StartFadeIn();

					break;

				case FadeType.FadeOut:
					StartFadeOut();

					break;
			}
		}

		private static void StartFadeIn()
		{
			fadeImage.SetTransparency(0);
			fadeImage.DOFade(1, fadeDurationAnim).SetEase(fadeEase).OnComplete(FadeInDone);
		}

		private static void StartFadeOut()
		{
			fadeImage.SetTransparency(1);
			fadeImage.DOFade(0, fadeDurationAnim).SetEase(fadeEase).OnComplete(FadeOutDone);
		}

		private static void FadeInDone()
		{
			OnFadeComplete(FadeType.FadeIn);
		}

		private static void FadeOutDone()
		{
			OnFadeComplete(FadeType.FadeOut);

			DisableFadeImage();
		}
	}
}