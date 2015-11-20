using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MultiDirectionMissileAction")]
public sealed class MultiDirectionMissileAction : MissileAction
{
	public class MissileInfo
	{
		public GameObject missile;

		public Vector3 direction = Vector3.zero;

		public float timer;
	}

	public int MissileCount = 5;

	public float MissileAngle = 15f;

	public float MissileLifeTime = 1.5f;

	public Vector3 Speed = new Vector3(3f, 3f, 3f);

	public Vector3 Accel = Vector3.zero;

	public Vector3 SpeedMax = new Vector3(1000f, 1000f, 1000f);

	private Vector3 speed;

	private List<MultiDirectionMissileAction.MissileInfo> missiles = new List<MultiDirectionMissileAction.MissileInfo>();

	protected override void DoAction()
	{
		if (base.variables.skillTarget != null)
		{
			base.variables.skillCaster.FaceToPosition(base.variables.skillTarget.transform.position);
		}
		if (this.MissileCount <= 1)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("MissileCount = {0} must > 1", this.MissileCount)
			});
			base.Finish();
			return;
		}
		if (this.MissilePrefab != null)
		{
			Vector3 position = base.variables.skillCaster.transform.position;
			position.y += this.YOffset;
			float num = base.variables.skillCaster.transform.rotation.eulerAngles.y;
			Vector3 pos = Vector3.zero;
			num -= this.MissileAngle * ((float)(this.MissileCount - 1) * 0.5f);
			for (int i = 0; i < this.MissileCount; i++)
			{
				float num2 = num + this.MissileAngle * (float)i;
				MultiDirectionMissileAction.MissileInfo missileInfo = new MultiDirectionMissileAction.MissileInfo();
				missileInfo.direction = new Vector3(Mathf.Sin(0.0174532924f * num2), 0f, Mathf.Cos(0.0174532924f * num2));
				pos = position + missileInfo.direction * this.ForwardOffset;
				Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, pos, Quaternion.LookRotation(missileInfo.direction), 1f);
				missileInfo.missile = transform.gameObject;
				if (missileInfo.missile == null)
				{
					global::Debug.LogError(new object[]
					{
						"Instantiate MissilePrefab object error!"
					});
				}
				else
				{
					missileInfo.timer = this.MissileLifeTime;
					this.missiles.Add(missileInfo);
				}
			}
		}
		this.speed = this.Speed;
		if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, base.variables.skillTarget.transform.position) <= this.ForwardOffset * this.ForwardOffset)
		{
			for (int j = 0; j < this.missiles.Count; j++)
			{
				if (this.missiles[j] != null)
				{
					this.OnExplode(this.missiles[j], base.variables.skillTarget);
				}
			}
			this.missiles.Clear();
			base.Finish();
		}
	}

	protected override void UpdateAction(float elapse)
	{
		Vector3 vector = this.Accel * elapse;
		this.speed.x = Mathf.Clamp(this.speed.x + vector.x, 1f, this.SpeedMax.x);
		this.speed.y = Mathf.Clamp(this.speed.y + vector.y, 1f, this.SpeedMax.y);
		this.speed.z = Mathf.Clamp(this.speed.z + vector.z, 1f, this.SpeedMax.z);
		Vector3 vector2 = this.speed * elapse;
		bool flag = true;
		for (int i = 0; i < this.missiles.Count; i++)
		{
			if (this.missiles[i] != null && this.missiles[i].missile != null)
			{
				this.missiles[i].timer -= elapse;
				if (this.missiles[i].timer <= 0f)
				{
					this.OnExplode(this.missiles[i], null);
				}
				else
				{
					this.missiles[i].missile.transform.position += this.missiles[i].direction * vector2.x;
					this.CheckExplode(this.missiles[i]);
				}
				flag = false;
			}
		}
		if (flag)
		{
			base.Finish();
		}
	}

	private void CheckExplode(MultiDirectionMissileAction.MissileInfo data)
	{
		ActorController actorController = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		float num = 0.1f;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController2 = actors[i];
			if (actorController2 && !actorController2.IsDead && base.variables.skillCaster.IsHostileTo(actorController2))
			{
				float num2 = CombatHelper.DistanceSquared2D(data.missile.transform.position, actorController2.transform.position);
				if (num2 < num)
				{
					num = num2;
					actorController = actorController2;
				}
			}
		}
		if (actorController != null)
		{
			this.OnExplode(data, actorController);
		}
	}

	private new void OnDespawned()
	{
		base.OnDespawned();
		for (int i = 0; i < this.missiles.Count; i++)
		{
			if (this.missiles[i] != null && this.missiles[i].missile != null)
			{
				PoolMgr.spawnPool.Despawn(this.missiles[i].missile.transform);
				this.missiles[i].missile = null;
			}
		}
		this.missiles.Clear();
	}

	private void OnExplode(MultiDirectionMissileAction.MissileInfo data, ActorController actor)
	{
		if (data != null && data.missile != null)
		{
			PoolMgr.spawnPool.Despawn(data.missile.transform, this.MissileDeleteDelay);
			data.missile = null;
		}
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, actor.transform.position, actor.transform.rotation, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		if (actor != null)
		{
			base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, actor, base.variables.targetPosition, 0);
		}
	}
}
