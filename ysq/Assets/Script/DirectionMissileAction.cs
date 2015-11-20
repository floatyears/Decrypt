using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/DirectionMissileAction")]
public sealed class DirectionMissileAction : MissileAction
{
	public float MissileLifeTime = 1.5f;

	public bool Spine;

	public Vector3 Speed = new Vector3(3f, 3f, 3f);

	public Vector3 Accel = Vector3.zero;

	public Vector3 SpeedMax = new Vector3(1000f, 1000f, 1000f);

	private Vector3 direction = Vector3.zero;

	private Vector3 speed;

	private float timer;

	protected override void DoAction()
	{
		base.CreateMissile();
		this.timer = this.MissileLifeTime;
		if (base.variables.skillTarget != null)
		{
			base.variables.skillCaster.FaceToPosition(base.variables.skillTarget.transform.position);
		}
		if (this.Spine && base.variables.skillTarget != null && base.variables.skillTarget.SpineTransform != null)
		{
			this.direction = base.variables.skillTarget.SpineTransform.position - base.transform.position;
		}
		else
		{
			this.direction = base.variables.skillCaster.transform.forward;
		}
		this.speed = this.Speed;
		if (this.missile != null && base.variables.skillTarget != null && CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, base.variables.skillTarget.transform.position) <= this.ForwardOffset * this.ForwardOffset)
		{
			this.OnExplode(base.variables.skillTarget);
		}
	}

	protected override void UpdateAction(float elapse)
	{
		if (this.missile == null)
		{
			base.Finish();
			return;
		}
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			this.OnExplode(null);
			return;
		}
		Vector3 vector = this.Accel * Time.deltaTime;
		this.speed.x = Mathf.Clamp(this.speed.x + vector.x, 1f, this.SpeedMax.x);
		this.speed.y = Mathf.Clamp(this.speed.y + vector.y, 1f, this.SpeedMax.y);
		this.speed.z = Mathf.Clamp(this.speed.z + vector.z, 1f, this.SpeedMax.z);
		Vector3 vector2 = this.speed * Time.deltaTime;
		this.missile.transform.position += this.direction * vector2.x;
		this.missile.transform.position += this.direction * vector2.y;
		this.missile.transform.position += this.direction * vector2.z;
		this.CheckExplode(this.missile.transform.position);
	}

	private void CheckExplode(Vector3 missilePos)
	{
		ActorController actorController = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		float num = 0.1f;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController2 = actors[i];
			if (actorController2 && !actorController2.IsDead && base.variables.skillCaster.IsHostileTo(actorController2))
			{
				float num2 = CombatHelper.DistanceSquared2D(missilePos, actorController2.transform.position);
				if (num2 < num)
				{
					num = num2;
					actorController = actorController2;
				}
			}
		}
		if (actorController != null)
		{
			this.OnExplode(actorController);
		}
	}

	private void OnExplode(ActorController actor)
	{
		if (this.missile != null)
		{
			PoolMgr.spawnPool.Despawn(this.missile.transform, this.MissileDeleteDelay);
			this.missile = null;
		}
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, actor.transform.position, actor.transform.rotation, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		if (actor != null)
		{
			base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, actor, base.variables.targetPosition, this.Index);
		}
	}
}
