using System;
using UnityEngine;

public sealed class MoveStateFear : MoveState
{
	private float timer;

	private Vector3 centerPos = Vector3.zero;

	private float radius;

	private float preAngle;

	public MoveStateFear(ActorController actor) : base(actor)
	{
	}

	public void SetCenterPos(Vector3 _centerPos, float _radius)
	{
		this.centerPos = _centerPos;
		this.radius = _radius;
	}

	private bool AdjustMove()
	{
		float num = UtilFunc.RangeRandom(1.04719758f, 5.235988f);
		num = this.preAngle + num;
		this.preAngle = num;
		float x = this.centerPos.x + this.radius * Mathf.Sin(num);
		float z = this.centerPos.z + this.radius * Mathf.Cos(num);
		if (this.actorCtrler.NavAgent != null && this.actorCtrler.CanMove(false, true))
		{
			this.actorCtrler.NavAgent.destination = new Vector3(x, this.centerPos.y, z);
		}
		return true;
	}

	public override void Enter()
	{
		this.AdjustMove();
	}

	public override bool Update(float elapse)
	{
		this.timer += elapse;
		if (this.timer >= 0.5f)
		{
			this.timer = 0f;
			return this.AdjustMove();
		}
		return true;
	}

	public override void Exit()
	{
	}
}
