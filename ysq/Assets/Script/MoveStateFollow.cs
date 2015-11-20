using System;
using UnityEngine;

public sealed class MoveStateFollow : MoveState
{
	public const float TimerInterval = 0.1f;

	private float timer;

	private ActorController target;

	private int slot;

	public MoveStateFollow(ActorController actor) : base(actor)
	{
	}

	public void SetTarget(ActorController _target, int _slot)
	{
		this.target = _target;
		this.slot = _slot;
	}

	private bool AdjustMove()
	{
		if (this.target == null || this.target.IsDead)
		{
			return true;
		}
		Vector3 slotPos = CombatHelper.GetSlotPos(this.target.transform.position, this.target.transform, this.slot, this.actorCtrler.AiCtrler.ForceFollow, false);
		if (this.actorCtrler.NavAgent.hasPath && this.actorCtrler.NavAgent.steeringTarget == slotPos)
		{
			return true;
		}
		float num = Vector3.Distance(this.actorCtrler.transform.position, slotPos);
		if (num > 1f)
		{
			if (this.slot > 200)
			{
				Quaternion a = Quaternion.LookRotation(slotPos - this.actorCtrler.transform.position);
				Quaternion quaternion = Quaternion.LookRotation(this.target.transform.position - this.actorCtrler.transform.position);
				float num2 = Quaternion.Angle(a, quaternion);
				float num3 = Vector3.Distance(this.actorCtrler.transform.position, this.target.transform.position);
				if ((num3 < 2f && num2 > 45f) || num3 < 1f)
				{
					this.actorCtrler.RotateTo(quaternion);
					this.actorCtrler.StopMove();
					return true;
				}
				this.actorCtrler.StartMove(slotPos);
			}
			else
			{
				this.actorCtrler.StartMove(slotPos);
			}
		}
		else if (this.actorCtrler.ActorType == ActorController.EActorType.ELopet)
		{
			Quaternion rotation = this.target.transform.rotation;
			this.actorCtrler.RotateTo(rotation);
		}
		float num4 = (this.actorCtrler.MaxRunSpeed > 0f) ? (this.target.MaxRunSpeed * this.target.SpeedScale / this.actorCtrler.MaxRunSpeed) : 1f;
		this.actorCtrler.UpdateSpeedScale(num4);
		num = Vector3.Distance(this.actorCtrler.transform.position, slotPos);
		if (num > 1.6f)
		{
			this.actorCtrler.UpdateSpeedScale(num4 * 1.8f);
		}
		return true;
	}

	public override void Enter()
	{
		this.timer = UtilFunc.RangeRandom(0f, 0.1f);
		this.AdjustMove();
	}

	public override bool Update(float elapse)
	{
		this.timer += elapse;
		if (this.timer >= 0.1f)
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
