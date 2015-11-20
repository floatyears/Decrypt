using System;
using UnityEngine;

[AddComponentMenu("Game/Action/PositionMissileAction")]
public class PositionMissileAction : MissileAction
{
	public float CreateMissileDelay = 0.2f;

	public float ExplodeDelayTime = 1f;

	private float timer;

	private bool flag;

	protected override void DoAction()
	{
		if (this.MissilePrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"MissilePrefab null"
			});
			base.Finish();
			return;
		}
		this.flag = false;
		this.timer = this.CreateMissileDelay;
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			if (!this.flag)
			{
				Vector3 targetPosition = base.variables.targetPosition;
				targetPosition.y += this.YOffset;
				Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, targetPosition, Quaternion.identity, 1f);
				this.missile = transform.gameObject;
				this.flag = true;
				this.timer = this.ExplodeDelayTime;
			}
			else
			{
				this.OnTouchTarget(base.variables.targetPosition);
				base.Finish();
			}
		}
	}
}
