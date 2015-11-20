using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MoveDamageAction")]
public sealed class MoveDamageAction : ActionBase
{
	public GameObject DamagePrefab;

	public float TickInterval = 1f;

	public int TickCount = 6;

	public float MoveSpeed = 1f;

	public float AroundSpeed = 1f;

	public float ForwardOffset = 0.3f;

	private ActorController moveTarget;

	private float tickTimer;

	private int index;

	private GameObject missile;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.index = 0;
		this.tickTimer = this.TickInterval;
		this.moveTarget = null;
		if (this.DamagePrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"DamagePrefab error!"
			});
			return;
		}
		Vector3 vector = base.variables.skillCaster.transform.position;
		if (base.variables.skillTarget != null)
		{
			vector = base.variables.skillTarget.transform.position;
			vector -= base.variables.skillCaster.transform.forward * this.ForwardOffset;
		}
		Transform transform = PoolMgr.SpawnParticleSystem(this.DamagePrefab.transform, vector, base.variables.skillCaster.transform.rotation, 1f);
		this.missile = transform.gameObject;
		if (this.missile == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate DamagePrefab object error!"
			});
			return;
		}
		this.MoveToNextTarget();
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null)
		{
			base.Finish();
			return;
		}
		this.tickTimer -= elapse;
		if (this.tickTimer <= 0f && this.index < this.TickCount)
		{
			this.index++;
			this.CastSkill();
			this.tickTimer += this.TickInterval;
			if (this.moveTarget == null || this.moveTarget.IsDead || !base.variables.skillCaster.IsHostileTo(this.moveTarget))
			{
				this.MoveToNextTarget();
			}
		}
		if (this.index >= this.TickCount)
		{
			base.Finish();
			return;
		}
		this.UpdateTransform(elapse);
	}

	private void UpdateTransform(float elapse)
	{
		if (this.moveTarget == null || this.missile == null)
		{
			return;
		}
		float num = this.MoveSpeed * elapse;
		Vector3 vector = this.moveTarget.transform.position - this.missile.transform.position;
		if (vector != Vector3.zero)
		{
			Quaternion to = Quaternion.LookRotation(vector);
			this.missile.transform.rotation = Quaternion.Slerp(this.missile.transform.rotation, to, Time.deltaTime * this.AroundSpeed);
		}
		if (vector.magnitude < num)
		{
			this.missile.transform.position = this.moveTarget.transform.position;
		}
		else
		{
			this.missile.transform.position += vector.normalized * num;
		}
	}

	private void MoveToNextTarget()
	{
		float num = 3.40282347E+38f;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
			{
				float num2 = CombatHelper.DistanceSquared2D(this.missile.transform.position, actorController.transform.position);
				if (base.variables.skillInfo == null || num2 <= base.variables.skillCaster.AiCtrler.AttackDistance)
				{
					if (num2 < num)
					{
						this.moveTarget = actorController;
						num = num2;
					}
				}
			}
		}
	}

	private void CastSkill()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
			{
				if (CombatHelper.DistanceSquared2D(this.missile.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
				{
					base.variables.skillCaster.DoEffectOnTarget(base.variables.skillInfo, actorController, 0);
				}
			}
		}
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		this.moveTarget = null;
		if (this.missile != null)
		{
			PoolMgr.spawnPool.Despawn(this.missile.transform);
			this.missile = null;
		}
	}
}
