using System;
using UnityEngine;

[AddComponentMenu("Game/Action/PauseAnimationAction")]
public sealed class PauseAnimationAction : ActionBase
{
	public float Duration;

	private float timer;

	protected override void DoAction()
	{
		if (base.variables.skillCaster.AnimationCtrler.AnimCtrl == null)
		{
			return;
		}
		base.variables.skillCaster.AnimationCtrler.AnimCtrl.enabled = false;
		this.timer = this.Duration;
		if (this.Duration <= 0f)
		{
			this.timer = 3.40282347E+38f;
			if (base.variables.action != null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("{0} Lift Time = {1} <= 0.0f", base.name, this.Duration)
				});
			}
		}
		else
		{
			this.timer = this.Duration;
		}
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			if (base.variables != null && base.variables.skillCaster != null && base.variables.skillCaster.AnimationCtrler.AnimCtrl != null && !base.variables.skillCaster.AnimationCtrler.AnimCtrl.enabled)
			{
				base.variables.skillCaster.AnimationCtrler.AnimCtrl.enabled = true;
			}
			base.Finish();
		}
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		if (base.variables != null && base.variables.skillCaster != null && base.variables.skillCaster.AnimationCtrler.AnimCtrl != null && !base.variables.skillCaster.AnimationCtrler.AnimCtrl.enabled)
		{
			base.variables.skillCaster.AnimationCtrler.AnimCtrl.enabled = true;
		}
	}
}
