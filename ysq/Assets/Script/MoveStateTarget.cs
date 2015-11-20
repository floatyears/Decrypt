using System;
using UnityEngine;

public sealed class MoveStateTarget : MoveState
{
	public const float TimerInterval = 0.2f;

	public ActorController Target;

	private float timer;

	private float distance;

	private float stop_timer;

	public MoveStateTarget(ActorController actor) : base(actor)
	{
	}

	public void SetTarget(ActorController target, float attackDistance)
	{
		if (this.Target != null)
		{
			this.Target.AiCtrler.ReleaseChaser(this.actorCtrler);
		}
		this.Target = target;
		this.distance = attackDistance;
	}

	public void Stop(float time)
	{
		this.stop_timer = time;
		this.actorCtrler.StopMove();
	}

	private bool AdjustMove()
	{
		if (this.Target == null || this.Target.IsDead)
		{
			return false;
		}
		if (!Globals.Instance.ActorMgr.IsPvpScene())
		{
			ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
			if (actor != null && !actor.IsDead && this.actorCtrler.AiCtrler != null && this.actorCtrler.AiCtrler.EnableAI && this.actorCtrler.ActorType == ActorController.EActorType.EPet && this.actorCtrler.AiCtrler.FindEnemyDistance < 20f && this.actorCtrler.GetDistance2D(actor) > 20f)
			{
				this.actorCtrler.AiCtrler.SetTarget(null);
				return false;
			}
		}
		float distance2D = this.actorCtrler.GetDistance2D(this.Target);
		if (this.Target.Unattacked)
		{
			if (distance2D > 1f)
			{
				Vector3 vector = this.Target.AiCtrler.GetChaserPos(this.actorCtrler, 1f);
				vector.y = this.actorCtrler.transform.position.y;
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(vector, out navMeshHit, 10f, -1))
				{
					vector = navMeshHit.position;
				}
				this.actorCtrler.StartMove(vector);
			}
		}
		else
		{
			float num = this.actorCtrler.GetBoundsRadius() + this.Target.GetBoundsRadius();
			if (distance2D > this.distance || (!this.actorCtrler.IsBoss && distance2D < 0.3f))
			{
				Vector3 vector2 = this.Target.AiCtrler.GetChaserPos(this.actorCtrler, this.distance + num);
				vector2.y = this.actorCtrler.transform.position.y;
				NavMeshHit navMeshHit2;
				if (NavMesh.SamplePosition(vector2, out navMeshHit2, 10f, -1))
				{
					vector2 = navMeshHit2.position;
				}
				if (Globals.Instance.SenceMgr.sceneInfo != null && Globals.Instance.SenceMgr.sceneInfo.Type == 0)
				{
					Vector3 position = this.Target.transform.position;
					Vector3 targetPosition = vector2;
					position.y += 0.7f;
					targetPosition.y += 0.7f;
					if (NavMesh.Raycast(position, targetPosition, out navMeshHit2, -1))
					{
						vector2 = navMeshHit2.position;
					}
				}
				this.actorCtrler.StartMove(vector2);
			}
		}
		return true;
	}

	public override void Enter()
	{
		this.timer = UtilFunc.RangeRandom(0f, 0.2f);
		this.AdjustMove();
	}

	public override bool Update(float elapse)
	{
		this.timer += elapse;
		this.stop_timer -= elapse;
		if (this.timer >= 0.2f && this.stop_timer <= 0f)
		{
			this.timer = 0f;
			return this.AdjustMove();
		}
		return true;
	}

	public override void Exit()
	{
		if (this.Target != null)
		{
			this.Target.AiCtrler.ReleaseChaser(this.actorCtrler);
		}
		this.Target = null;
	}
}
