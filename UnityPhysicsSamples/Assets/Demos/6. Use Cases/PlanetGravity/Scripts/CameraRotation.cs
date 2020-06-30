using DG.Tweening;
using UnityEngine;

namespace Demos._6._Use_Cases.PlanetGravity.Scripts
{
	public class CameraRotation : MonoBehaviour
	{
		private void Start()
		{
			transform.DORotate(Vector3.up * 35f, 4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
			transform.DOScale(Vector3.one * 1.15f, 4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		}
	}
}