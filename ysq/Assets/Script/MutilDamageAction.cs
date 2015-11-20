using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MutilDamageAction")]
public sealed class MutilDamageAction : ActionBase
{
	public List<float> DelayDamages = new List<float>();

	private float timer;

	private int index;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.index = 0;
		if (this.index >= this.DelayDamages.Count)
		{
			base.Finish();
			return;
		}
		this.timer = this.DelayDamages[this.index] / base.variables.skillCaster.AttackSpeed;
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted())
		{
			base.Finish();
			return;
		}
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			base.variables.skillCaster.OnSkillCast(base.variables.skillInfo, base.variables.skillTarget, base.variables.targetPosition, this.index);
			this.index++;
			if (this.index >= this.DelayDamages.Count)
			{
				base.Finish();
				return;
			}
			this.timer = this.DelayDamages[this.index] / base.variables.skillCaster.AttackSpeed;
		}
	}
}
