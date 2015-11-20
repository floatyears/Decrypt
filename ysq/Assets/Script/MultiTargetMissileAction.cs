using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MultiTargetMissileAction")]
public class MultiTargetMissileAction : MissileAction
{
	public enum CurveType
	{
		Normal,
		Bezier
	}

	public class TargetInfo
	{
		public GameObject missile;

		public ActorController actor;

		public GameObject target;

		public Vector3 targetPosition = Vector3.zero;

		public float distanceToTarget;

		public Vector3 speed = Vector3.one;

		public Bezier curveBezier;

		public float timeStamp;

		public float duration;
	}

	public bool Spine = true;

	public Vector3 Speed = Vector3.one;

	public Vector3 Accel = Vector3.zero;

	public Vector3 SpeedMax = new Vector3(1000f, 1000f, 1000f);

	public float damping = 3f;

	public float parabolaAngle;

	public MultiTargetMissileAction.CurveType curveType;

	public Vector3 BezierOffest = new Vector3(0f, 5f, 0f);

	public float BezierDutationMin = 0.25f;

	private List<MultiTargetMissileAction.TargetInfo> targets = new List<MultiTargetMissileAction.TargetInfo>();

	protected override void DoAction()
	{
		switch (base.variables.skillInfo.EffectTargetType)
		{
		case 0:
			if (base.variables.skillInfo.CastTargetType != 3)
			{
				this.CraeteMissileToTarget(base.variables.skillTarget);
			}
			return;
		case 2:
		{
			List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorController actorController = actors[i];
				if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
					{
						this.CraeteMissileToTarget(actorController);
					}
				}
			}
			return;
		}
		case 3:
		{
			List<ActorController> actors2 = Globals.Instance.ActorMgr.Actors;
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorController actorController = actors2[j];
				if (actorController && !actorController.IsDead && base.variables.skillCaster.IsFriendlyTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
					{
						this.CraeteMissileToTarget(actorController);
					}
				}
			}
			return;
		}
		case 4:
		{
			List<ActorController> actors3 = Globals.Instance.ActorMgr.Actors;
			for (int k = 0; k < actors3.Count; k++)
			{
				ActorController actorController = actors3[k];
				if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
					{
						Quaternion rotation = base.variables.skillCaster.transform.rotation;
						rotation.x = 0f;
						rotation.z = 0f;
						Quaternion b = Quaternion.LookRotation(actorController.transform.position - base.variables.skillCaster.transform.position);
						b.x = 0f;
						b.z = 0f;
						float num = Quaternion.Angle(rotation, b);
						if (num <= base.variables.skillInfo.Angle * 0.5f)
						{
							this.CraeteMissileToTarget(actorController);
						}
					}
				}
			}
			return;
		}
		}
		global::Debug.LogError(new object[]
		{
			string.Concat(new object[]
			{
				"Not support SkillID = [",
				base.variables.skillInfo.ID,
				"] invalid EffectTargetType = [",
				base.variables.skillInfo.EffectTargetType,
				"]"
			})
		});
		base.Finish();
	}

	private void CraeteMissileToTarget(ActorController actor)
	{
		if (this.MissilePrefab == null || base.variables.skillCaster == null || actor == null || actor.IsDead)
		{
			return;
		}
		if (base.variables.skillCaster.GetDistance2D(actor) > base.variables.skillInfo.Radius)
		{
			return;
		}
		Vector3 vector = base.variables.skillCaster.transform.position;
		vector.y += this.YOffset;
		vector += base.variables.skillCaster.transform.forward * this.ForwardOffset;
		vector += base.variables.skillCaster.transform.right * this.RightOffset;
		Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, this.BezierOffest, base.variables.skillCaster.transform.rotation, 1f);
		if (transform == null || transform.gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Instantiate MissilePrefab object error!"
			});
			return;
		}
		GameObject gameObject;
		if (this.Spine && actor.SpineTransform != null)
		{
			gameObject = actor.SpineTransform.gameObject;
		}
		else
		{
			gameObject = actor.gameObject;
		}
		MultiTargetMissileAction.TargetInfo targetInfo = new MultiTargetMissileAction.TargetInfo();
		targetInfo.actor = actor;
		targetInfo.target = gameObject;
		targetInfo.targetPosition = gameObject.transform.position;
		targetInfo.speed = this.Speed;
		targetInfo.missile = transform.gameObject;
		targetInfo.distanceToTarget = Vector3.Distance(targetInfo.missile.transform.position, targetInfo.targetPosition);
		targetInfo.timeStamp = Time.time;
		if (this.curveType == MultiTargetMissileAction.CurveType.Bezier)
		{
			targetInfo.curveBezier = new Bezier(vector, base.transform.right * this.BezierOffest.x + Vector3.up * this.BezierOffest.y + base.transform.forward * this.BezierOffest.z, Vector3.zero, targetInfo.targetPosition);
			targetInfo.duration = ((targetInfo.speed.magnitude > 0f) ? Mathf.Max(this.BezierDutationMin, targetInfo.distanceToTarget / targetInfo.speed.magnitude) : 0f);
		}
		this.targets.Add(targetInfo);
	}

	private void UpdateTarget(MultiTargetMissileAction.TargetInfo targetInfo)
	{
		if (targetInfo == null || targetInfo.missile == null)
		{
			return;
		}
		if (this.curveType == MultiTargetMissileAction.CurveType.Bezier)
		{
			float num = Time.time - targetInfo.timeStamp;
			if (num > targetInfo.duration)
			{
				this.OnReachTarget(targetInfo);
				return;
			}
			float num2 = num / this.BezierDutationMin;
			num2 *= num2;
			Vector3 pointAtTime = targetInfo.curveBezier.GetPointAtTime(num2);
			targetInfo.missile.transform.position = pointAtTime;
			Vector3 forward = targetInfo.targetPosition - pointAtTime;
			if (forward.sqrMagnitude > 0.1f)
			{
				Quaternion to = Quaternion.LookRotation(forward);
				this.damping += 0.9f;
				this.missile.transform.rotation = Quaternion.Slerp(this.missile.transform.rotation, to, Time.deltaTime * this.damping);
			}
		}
		else
		{
			if (targetInfo.target != null && !targetInfo.actor.IsDead)
			{
				targetInfo.targetPosition = targetInfo.target.transform.position;
				targetInfo.targetPosition.y = targetInfo.targetPosition.y + this.YOffset;
			}
			Vector3 vector = this.Accel * Time.deltaTime;
			targetInfo.speed.x = Mathf.Clamp(targetInfo.speed.x + vector.x, 1f, this.SpeedMax.x);
			targetInfo.speed.y = Mathf.Clamp(targetInfo.speed.y + vector.y, 1f, this.SpeedMax.y);
			targetInfo.speed.z = Mathf.Clamp(targetInfo.speed.z + vector.z, 1f, this.SpeedMax.z);
			if (this.parabolaAngle <= 0f)
			{
				Vector3 vector2 = targetInfo.targetPosition - targetInfo.missile.transform.position;
				if (vector2 != Vector3.zero)
				{
					Quaternion to2 = Quaternion.LookRotation(vector2);
					this.damping += 0.9f;
					targetInfo.missile.transform.rotation = Quaternion.Slerp(targetInfo.missile.transform.rotation, to2, Time.deltaTime * this.damping);
				}
				Vector3 vector3 = targetInfo.speed * Time.deltaTime;
				targetInfo.missile.transform.position += vector2.normalized * vector3.x;
				targetInfo.missile.transform.position += vector2.normalized * vector3.y;
				targetInfo.missile.transform.position += vector2.normalized * vector3.z;
				if (vector2.magnitude < vector3.magnitude)
				{
					targetInfo.targetPosition.y = targetInfo.targetPosition.y - this.YOffset;
					this.OnReachTarget(targetInfo);
					return;
				}
			}
			else
			{
				targetInfo.missile.transform.LookAt(targetInfo.targetPosition);
				float num3 = targetInfo.speed.x * targetInfo.speed.y * targetInfo.speed.z * Time.deltaTime;
				float num4 = Vector3.Distance(targetInfo.missile.transform.position, targetInfo.targetPosition);
				float num5 = (targetInfo.distanceToTarget > 0f) ? (Mathf.Min(1f, num4 / targetInfo.distanceToTarget) * this.parabolaAngle) : 0f;
				num5 = Mathf.Clamp(-num5, -this.parabolaAngle, this.parabolaAngle);
				targetInfo.missile.transform.rotation = targetInfo.missile.transform.rotation * Quaternion.Euler(num5, 0f, 0f);
				targetInfo.missile.transform.Translate(Vector3.forward * num3);
				if (num4 < num3)
				{
					targetInfo.targetPosition.y = targetInfo.targetPosition.y - this.YOffset;
					this.OnReachTarget(targetInfo);
					return;
				}
			}
		}
	}

	private void OnReachTarget(MultiTargetMissileAction.TargetInfo targetInfo)
	{
		if (targetInfo == null)
		{
			return;
		}
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, targetInfo.missile.transform.position, Quaternion.identity, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		if (targetInfo.missile != null)
		{
			PoolMgr.spawnPool.Despawn(targetInfo.missile.transform, this.MissileDeleteDelay);
			targetInfo.missile = null;
		}
		if (base.variables != null && base.variables.skillCaster != null && targetInfo.actor != null)
		{
			base.variables.skillCaster.DoEffectOnTarget(base.variables.skillInfo, targetInfo.actor, 0);
		}
	}

	protected override void UpdateAction(float elapse)
	{
		bool flag = true;
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] == null || this.targets[i].missile == null)
			{
				flag = false;
			}
			else
			{
				this.UpdateTarget(this.targets[i]);
			}
		}
		if (flag)
		{
			base.Finish();
		}
	}

	private new void OnDespawned()
	{
		base.OnDespawned();
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] != null && this.targets[i].missile != null)
			{
				PoolMgr.spawnPool.Despawn(this.targets[i].missile.transform);
				this.targets[i].missile = null;
			}
		}
		this.targets.Clear();
	}
}
