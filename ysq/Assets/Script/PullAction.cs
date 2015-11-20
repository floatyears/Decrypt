using System;
using UnityEngine;

[AddComponentMenu("Game/Action/PullAction")]
public class PullAction : ActionBase
{
	public float initSpeed = 1f;

	public float maxSpeed = 20f;

	public float acceleration = 80f;

	private Vector3 direction;

	private float targetDis;

	private float speed;

	protected override void DoAction()
	{
		if (base.variables.skillTarget == null)
		{
			base.Finish();
			return;
		}
		this.speed = this.initSpeed;
		base.variables.skillCaster.FaceToPosition(base.variables.skillTarget.transform.position);
		this.direction = Vector3.Normalize(base.variables.skillTarget.transform.position - base.variables.skillCaster.transform.position);
		this.targetDis = Vector3.Distance(base.variables.skillTarget.transform.position, base.variables.skillCaster.transform.position);
		if (this.targetDis < 0.5f)
		{
			base.Finish();
			return;
		}
		this.targetDis -= 0.5f;
		base.variables.skillCaster.BuffRoot(true);
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted() || base.variables.skillTarget == null || base.variables.skillCaster.NavAgent == null)
		{
			base.variables.skillCaster.BuffRoot(false);
			base.Finish();
			return;
		}
		this.speed += this.acceleration * elapse;
		if (this.speed > this.maxSpeed)
		{
			this.speed = this.maxSpeed;
		}
		this.targetDis -= elapse * this.speed;
		if (this.targetDis < 0f)
		{
			base.variables.skillCaster.BuffRoot(false);
			base.Finish();
			return;
		}
		Vector3 b = elapse * this.speed * this.direction;
		b.y = 0f;
		Vector3 vector = base.variables.skillCaster.transform.position + b;
		NavMeshHit navMeshHit;
		if (base.variables.skillCaster.NavAgent.Raycast(vector, out navMeshHit))
		{
			vector = navMeshHit.position;
			base.variables.skillCaster.BuffRoot(false);
			base.Finish();
			return;
		}
		base.variables.skillCaster.NavAgent.Warp(vector);
	}
}
