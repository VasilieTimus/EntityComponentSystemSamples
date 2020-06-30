using DG.Tweening;
using UnityEngine;

namespace Demos._6._Use_Cases.PlanetGravity.Scripts
{
	public class CameraRotation : MonoBehaviour
	{
		private void Start()
		{
			transform.DORotate(Vector3.up * 35f, 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
			transform.DOScale(Vector3.one * 1.5f, 5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		}
	}
}