using System;
using UnityEngine;

[AddComponentMenu("Game/Action/ChargeForwardAction")]
public class ChargeForwardAction : ActionBase
{
	public float initSpeed = 1f;

	public float maxSpeed = 20f;

	public float acceleration = 80f;

	public float distance = 2f;

	private Vector3 direction;

	private float targetDis;

	private float speed;

	protected override void DoAction()
	{
		this.speed = this.initSpeed;
		this.direction = base.variables.skillCaster.transform.forward;
		this.targetDis = this.distance;
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted() || base.variables.skillCaster.NavAgent == null)
		{
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
			base.Finish();
			return;
		}
		base.variables.skillCaster.NavAgent.Warp(vector);
	}
}
