using System;
using UnityEngine;

public sealed class MoveStatePoint : MoveState
{
	public const float TimerInterval = 0.2f;

	private float timer;

	private Vector3 targetPos = Vector3.zero;

	private int pointID;

	private float timeout = 2f;

	private float timer2;

	private bool flag;

	public MoveStatePoint(ActorController actor) : base(actor)
	{
	}

	public void SetTarget(Vector3 _targetPos, int _pointID, float _timeout)
	{
		this.targetPos = _targetPos;
		this.pointID = _pointID;
		this.timeout = _timeout;
		this.timer2 = 0f;
		this.flag = false;
	}

	private bool AdjustMove()
	{
		if (this.actorCtrler.NavAgent.velocity.sqrMagnitude <= 0f)
		{
			float num = Vector3.Distance(this.actorCtrler.transform.position, this.targetPos);
			if (num <= 0.5f)
			{
				this.actorCtrler.AiCtrler.OnArrivedPoint(this.pointID, false);
				this.flag = true;
				return false;
			}
			this.actorCtrler.StartMove(this.targetPos);
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
		this.timer2 += elapse;
		if (this.timer2 > this.timeout)
		{
			this.actorCtrler.AiCtrler.OnArrivedPoint(this.pointID, true);
			this.flag = true;
			return false;
		}
		if (this.timer >= 0.2f)
		{
			this.timer = 0f;
			return this.AdjustMove();
		}
		return true;
	}

	public override void Exit()
	{
		if (!this.flag)
		{
			this.actorCtrler.AiCtrler.OnArrivedPoint(this.pointID, false);
			this.flag = true;
		}
	}
}
