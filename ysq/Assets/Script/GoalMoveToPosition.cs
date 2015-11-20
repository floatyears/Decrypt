using System;
using UnityEngine;

public sealed class GoalMoveToPosition : GoalBase
{
	private PlayerController playerCtrl;

	private Vector3 targetPos = Vector3.zero;

	private float threshold;

	private float timer;

	private NavMeshPath path = new NavMeshPath();

	private bool touch;

	public GoalMoveToPosition(ActorController actor, PlayerController player) : base(actor)
	{
		this.playerCtrl = player;
	}

	public bool SetTargetPosition(Vector3 _targetPos, float _threshold = 0.2f)
	{
		if (!this.actorCtrler.NavAgent.CalculatePath(_targetPos, this.path) || this.path.status != NavMeshPathStatus.PathComplete)
		{
			return false;
		}
		this.targetPos = _targetPos;
		this.threshold = _threshold;
		if (this.TryTestGoal())
		{
			return true;
		}
		this.playerCtrl.ShowTouchDownEffect(true, _targetPos);
		this.actorCtrler.AiCtrler.Locked = true;
		return false;
	}

	public void SetTouchMovePosition(Vector3 _targetPos)
	{
		if (!this.actorCtrler.NavAgent.CalculatePath(_targetPos, this.path) || this.path.status != NavMeshPathStatus.PathComplete)
		{
			return;
		}
		this.touch = true;
		this.targetPos = _targetPos;
		if (this.actorCtrler.CanMove(true, false))
		{
			this.actorCtrler.InterruptSkill(0);
			this.actorCtrler.AnimationCtrler.StopAnimation();
			this.actorCtrler.StartMove(_targetPos);
		}
	}

	public void SetTouchUp(bool showEffect)
	{
		this.touch = false;
		if (showEffect)
		{
			this.playerCtrl.ShowTouchDownEffect(true, this.targetPos);
		}
	}

	public override bool Update(float elapse)
	{
		if (this.actorCtrler == null || this.actorCtrler.IsDead)
		{
			return true;
		}
		this.timer += elapse;
		if (this.touch || this.timer < 0.1f)
		{
			return false;
		}
		this.timer = 0f;
		return this.TryTestGoal();
	}

	public override void OnInterrupt()
	{
		this.actorCtrler.AiCtrler.Locked = false;
		this.playerCtrl.ShowTouchDownEffect(false, Vector3.zero);
	}

	private bool TryTestGoal()
	{
		if (this.actorCtrler.IsDead || this.actorCtrler.IsRoot || this.actorCtrler.IsStun)
		{
			this.OnInterrupt();
			return true;
		}
		float num = Vector3.Distance(this.actorCtrler.transform.position, this.targetPos);
		if (num > this.threshold)
		{
			if (this.actorCtrler.CanMove(true, false))
			{
				this.actorCtrler.AiCtrler.SetSelectTarget(null);
				this.actorCtrler.InterruptSkill(0);
				this.actorCtrler.AnimationCtrler.StopAnimation();
				this.actorCtrler.StartMove(this.targetPos);
			}
			return false;
		}
		this.actorCtrler.AiCtrler.Locked = false;
		this.playerCtrl.ShowTouchDownEffect(false, Vector3.zero);
		return true;
	}
}
