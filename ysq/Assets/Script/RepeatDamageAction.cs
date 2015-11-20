using System;
using UnityEngine;

[AddComponentMenu("Game/Action/RepeatDamageAction")]
public sealed class RepeatDamageAction : ActionBase
{
	public float TickInterval = 0.5f;

	public int TickCount = 6;

	private float timer;

	private int index;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.timer = 0f;
		this.index = 0;
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted() || this.index >= this.TickCount)
		{
			base.Finish();
			return;
		}
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			base.variables.skillCaster.OnSkillCast(base.variables.skillInfo, base.variables.skillTarget, base.variables.targetPosition, this.index);
			this.timer = this.TickInterval / base.variables.skillCaster.AttackSpeed;
			this.index++;
		}
	}
}
