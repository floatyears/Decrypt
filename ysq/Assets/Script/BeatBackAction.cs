using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Beat Back Action")]
public class BeatBackAction : ActionBase
{
	public float initSpeed = 20f;

	public float acceleration = 80f;

	private NavMeshHit navHit = default(NavMeshHit);

	private Vector3 initBackDir;

	private float speed;

	protected override void DoAction()
	{
		this.speed = this.initSpeed;
		base.variables.skillCaster.StopMove();
		if (base.variables.skillTarget != null)
		{
			base.variables.skillCaster.FaceToPosition(base.variables.skillTarget.transform.position);
			this.initBackDir = Vector3.Normalize(base.variables.skillCaster.transform.position - base.variables.skillTarget.transform.position);
		}
		else
		{
			this.initBackDir = -base.variables.skillCaster.transform.forward;
		}
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables.skillCaster == null || this.speed <= 0f || base.variables.skillCaster.NavAgent == null)
		{
			base.Finish();
			return;
		}
		this.speed -= this.acceleration * elapse;
		Vector3 b = elapse * this.speed * this.initBackDir;
		b.y = 0f;
		Vector3 vector = base.variables.skillCaster.transform.position + b;
		if (base.variables.skillCaster.NavAgent.Raycast(vector, out this.navHit))
		{
			vector = this.navHit.position;
		}
		base.variables.skillCaster.NavAgent.Warp(vector);
	}
}
