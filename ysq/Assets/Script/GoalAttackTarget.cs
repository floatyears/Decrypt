using System;
using UnityEngine;

public sealed class GoalAttackTarget : GoalBase
{
	private ActorController target;

	public int skillIndex;

	private float timer;

	private bool focusTarget;

	public GoalAttackTarget(ActorController actor) : base(actor)
	{
	}

	public bool SetAttackTarget(ActorController _target, int _skillIndex, bool _focusTarget)
	{
		if (_target == null || _target.IsDead)
		{
			return true;
		}
		this.target = _target;
		this.skillIndex = _skillIndex;
		this.focusTarget = _focusTarget;
		this.actorCtrler.AiCtrler.Locked = true;
		if (this.focusTarget)
		{
			Globals.Instance.ActorMgr.SetSelectTarget(_target);
		}
		return this.TryTestGoal();
	}

	public override bool Update(float elapse)
	{
		this.timer += elapse;
		if (this.timer < 0.2f)
		{
			return false;
		}
		this.timer = 0f;
		if (this.target == null || this.target.IsDead)
		{
			this.OnInterrupt();
			return true;
		}
		return this.TryTestGoal();
	}

	public override void OnInterrupt()
	{
		if (this.focusTarget)
		{
			this.actorCtrler.AiCtrler.SetSelectTarget(null);
		}
		this.actorCtrler.AiCtrler.Locked = false;
	}

	private bool TryTestGoal()
	{
		float distance2D = this.actorCtrler.GetDistance2D(this.target);
		if (distance2D <= this.actorCtrler.AiCtrler.AttackDistance)
		{
			if (this.actorCtrler.Skills[this.skillIndex].Info.CastTargetType == 3)
			{
				Vector3 playerAOEPos = AIController.GetPlayerAOEPos(this.target, this.actorCtrler.FactionType);
				this.actorCtrler.TryCastSkill(this.skillIndex, playerAOEPos);
			}
			else
			{
				this.actorCtrler.TryCastSkill(this.skillIndex, this.target);
			}
			this.actorCtrler.AiCtrler.Locked = false;
			return true;
		}
		Vector3 vector = this.target.AiCtrler.GetChaserPos(this.actorCtrler, (this.actorCtrler.AiCtrler.AttackDistance + this.actorCtrler.GetBoundsRadius() + this.target.GetBoundsRadius()) * 0.85f);
		vector.y = this.actorCtrler.transform.position.y;
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(vector, out navMeshHit, 10f, -1))
		{
			vector = navMeshHit.position;
		}
		this.actorCtrler.StartMove(vector);
		return false;
	}
}
