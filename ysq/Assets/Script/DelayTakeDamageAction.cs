using System;
using UnityEngine;

[AddComponentMenu("Game/Action/DelayTakeDamageAction")]
public sealed class DelayTakeDamageAction : ActionBase
{
	public float CastDelayTime;

	public int Index;

	private float timer;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.timer = this.CastDelayTime / base.variables.skillCaster.AttackSpeed;
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			if (base.variables != null && base.variables.skillCaster != null)
			{
				if (this.Index > 0 && base.variables.skillInfo.CastTargetType == 3 && base.variables.skillInfo.EffectTargetType == 0)
				{
					base.transform.rotation = Quaternion.Euler(0f, base.variables.skillCaster.SkillValue, 0f);
					Vector3 targetPosition = base.variables.targetPosition - base.transform.right * ((float)this.Index * base.variables.skillInfo.Weight);
					base.variables.skillCaster.OnSkillCast(base.variables.skillInfo, null, targetPosition, this.Index);
				}
				else
				{
					base.variables.skillCaster.OnSkillCast(base.variables.skillInfo, base.variables.skillTarget, base.variables.targetPosition, this.Index);
				}
			}
			base.Finish();
		}
	}
}
