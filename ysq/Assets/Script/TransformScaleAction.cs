using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Transform Scale Action")]
public sealed class TransformScaleAction : ActionBase
{
	public float scale = 1f;

	public float duration = 1f;

	private float timer;

	protected override void DoAction()
	{
		if (this.scale == 0f)
		{
			this.scale = 1f;
		}
		if (base.variables.skillCaster != null)
		{
			base.variables.skillCaster.ActionScale = base.variables.skillCaster.ActionScale * this.scale;
		}
		this.timer = this.duration;
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			if (base.variables.skillCaster != null)
			{
				base.variables.skillCaster.ActionScale = base.variables.skillCaster.ActionScale / this.scale;
			}
			base.Finish();
		}
	}
}
