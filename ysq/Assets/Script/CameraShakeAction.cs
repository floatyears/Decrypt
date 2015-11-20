using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Camera Shake Action")]
public sealed class CameraShakeAction : ActionBase
{
	public int numberOfShakes = 2;

	public Vector3 shakeAmount = Vector3.one;

	public Vector3 rotationAmount = Vector3.one;

	public float distance = 0.1f;

	public float speed = 50f;

	public float decay = 0.2f;

	public bool multiplyByTimeScale = true;

	protected override void DoAction()
	{
		if (CameraShake.Instance != null)
		{
			CameraShake.CancelShake();
			CameraShake.Shake(this.numberOfShakes, this.shakeAmount, this.rotationAmount, this.distance, this.speed, this.decay, 1f, true);
		}
		base.Finish();
	}
}
