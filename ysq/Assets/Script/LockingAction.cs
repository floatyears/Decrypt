using System;
using UnityEngine;

[AddComponentMenu("Game/Action/LockingAction")]
public sealed class LockingAction : ActionBase
{
	public float Duration;

	public bool CanMove;

	public bool ImmuneControl;

	private float timer;

	private bool stopLoopAnim;

	protected override void DoAction()
	{
		base.variables.skillCaster.OnLockingStart(this.CanMove, this.ImmuneControl);
		this.timer = this.Duration / base.variables.skillCaster.AttackSpeed;
	}

	protected override void UpdateAction(float elapse)
	{
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			if (base.variables != null && !base.variables.IsInterrupted())
			{
				if (this.stopLoopAnim)
				{
					base.variables.skillCaster.AnimationCtrler.StopAnimation();
					this.stopLoopAnim = false;
				}
				base.variables.skillCaster.OnLockingStop(base.variables.skillInfo);
			}
			base.Finish();
		}
	}

	public void SetStopLoopAnim(bool value)
	{
		this.stopLoopAnim = value;
	}
}
