using System;
using UnityEngine;

[AddComponentMenu("Game/Action/ChargeTargetAction")]
public class ChargeTargetAction : ActionBase
{
	public float initSpeed = 1f;

	public float maxSpeed = 20f;

	public float acceleration = 80f;

	public PlayAnimation anim = new PlayAnimation();

	private float speed;

	private bool reachTarget;

	private float damping = 3f;

	private float timer = 0.5f;

	protected override void DoAction()
	{
		if (base.variables.skillTarget == null)
		{
			base.Finish();
			return;
		}
		base.variables.skillCaster.AnimationCtrler.PlayAnimation(this.anim);
		base.variables.skillCaster.OnLockingStart(false, true);
		this.speed = this.initSpeed;
		this.reachTarget = false;
	}

	private void FinishAction()
	{
		base.variables.skillCaster.AnimationCtrler.StopAnimation();
		base.variables.skillCaster.OnLockingStop(base.variables.skillInfo);
		base.Finish();
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted() || base.variables.skillTarget == null || base.variables.skillCaster.NavAgent == null)
		{
			this.FinishAction();
			return;
		}
		if (!this.reachTarget)
		{
			this.speed += this.acceleration * elapse;
			if (this.speed > this.maxSpeed)
			{
				this.speed = this.maxSpeed;
			}
			Vector3 vector = base.variables.skillTarget.transform.position - base.variables.skillCaster.transform.position;
			if (vector != Vector3.zero)
			{
				Quaternion to = Quaternion.LookRotation(vector);
				this.damping += 0.9f;
				base.variables.skillCaster.transform.rotation = Quaternion.Slerp(base.variables.skillCaster.transform.rotation, to, elapse * this.damping);
			}
			float num = elapse * this.speed;
			Vector3 vector2 = base.variables.skillCaster.transform.position + vector.normalized * num;
			NavMeshHit navMeshHit;
			if (base.variables.skillCaster.NavAgent.Raycast(vector2, out navMeshHit))
			{
				vector2 = navMeshHit.position;
				this.reachTarget = true;
				this.timer = 0.5f;
			}
			Vector3 a = base.variables.skillTarget.transform.position - vector.normalized * 0.8f;
			float num2 = Vector3.Distance(a, base.variables.skillCaster.transform.position);
			if (num2 < num)
			{
				this.reachTarget = true;
				this.timer = 0.5f;
			}
			base.variables.skillCaster.NavAgent.Warp(vector2);
			if (this.reachTarget)
			{
				base.variables.skillCaster.OnSkillCast(base.variables.skillInfo, base.variables.skillTarget, base.variables.skillCaster.transform.position, 0);
			}
		}
		else
		{
			this.timer -= elapse;
			if (this.timer < 0f)
			{
				this.FinishAction();
			}
		}
	}
}
