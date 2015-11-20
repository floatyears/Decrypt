using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MultiBezierMissilesAction")]
public sealed class MultiBezierMissilesAction : ActionBase
{
	public enum MoveType
	{
		StartToTarget,
		TargetToStart
	}

	[Serializable]
	public class MissileContent
	{
		public Vector3 offest;

		public float speed = 10f;

		public float DurationMin = 0.25f;
	}

	public GameObject MissilePrefab;

	public float YOffset;

	public float ForwardOffset;

	public float RightOffset;

	public bool Spine = true;

	public float MissileDeleteDelay = 0.1f;

	public MultiBezierMissilesAction.MoveType moveType;

	public List<MultiBezierMissilesAction.MissileContent> missileOffests = new List<MultiBezierMissilesAction.MissileContent>();

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	private GameObject target;

	private Vector3 startPosition = Vector3.zero;

	private Vector3 targetPosition = Vector3.zero;

	private float timer;

	private List<GameObject> missileInstances = new List<GameObject>();

	private List<float> missileDuration = new List<float>();

	private float durationMax;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		if (this.missileOffests.Count <= 0 || this.MissilePrefab == null)
		{
			base.Finish();
			return;
		}
		this.startPosition = base.variables.skillCaster.transform.position;
		this.startPosition.y = this.startPosition.y + this.YOffset;
		if (base.variables.skillTarget != null)
		{
			if (this.Spine && base.variables.skillTarget.SpineTransform != null)
			{
				this.target = base.variables.skillTarget.SpineTransform.gameObject;
			}
			else
			{
				this.target = base.variables.skillTarget.gameObject;
			}
		}
		if (this.target == null)
		{
			this.targetPosition = base.variables.skillCaster.transform.position + base.variables.skillCaster.transform.forward * (base.variables.skillCaster.AiCtrler.AttackDistance + 1.5f);
		}
		else
		{
			this.targetPosition = this.target.transform.position;
		}
		this.targetPosition.y = this.targetPosition.y + this.YOffset;
		float num = Vector3.Distance(this.startPosition, this.targetPosition);
		if (this.moveType == MultiBezierMissilesAction.MoveType.StartToTarget || this.target == null)
		{
			this.startPosition += base.variables.skillCaster.transform.forward * this.ForwardOffset;
			this.startPosition += base.variables.skillCaster.transform.right * this.RightOffset;
		}
		else if (this.target != null)
		{
			this.targetPosition += this.target.transform.forward * this.ForwardOffset;
			this.targetPosition += this.target.transform.right * this.RightOffset;
		}
		this.missileInstances.Clear();
		this.missileDuration.Clear();
		this.durationMax = 0f;
		for (int i = 0; i < this.missileOffests.Count; i++)
		{
			Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, this.startPosition, base.variables.skillCaster.transform.rotation, 1f);
			if (transform == null)
			{
				global::Debug.LogError(new object[]
				{
					"Instantiate MissilePrefab object error!"
				});
			}
			this.missileInstances.Add(transform.gameObject);
			float num2 = Mathf.Max(this.missileOffests[i].DurationMin, num / this.missileOffests[i].speed);
			this.missileDuration.Add(num2);
			this.durationMax = Mathf.Max(num2, this.durationMax);
		}
		this.timer = 0f;
	}

	protected override void UpdateAction(float elapse)
	{
		if (this.missileInstances.Count == 0)
		{
			base.Finish();
			return;
		}
		this.timer += elapse;
		if (this.timer > this.durationMax)
		{
			this.OnReachTarget();
			return;
		}
		if (this.target != null && !base.variables.skillTarget.IsDead)
		{
			this.targetPosition = this.target.transform.position;
			this.targetPosition.y = this.targetPosition.y + this.YOffset;
			if (this.moveType == MultiBezierMissilesAction.MoveType.TargetToStart)
			{
				this.targetPosition += this.target.transform.forward * this.ForwardOffset;
				this.targetPosition += this.target.transform.right * this.RightOffset;
			}
		}
		for (int i = 0; i < this.missileInstances.Count; i++)
		{
			GameObject gameObject = this.missileInstances[i];
			if (!(gameObject == null))
			{
				if (this.timer > this.missileDuration[i])
				{
					PoolMgr.spawnPool.Despawn(gameObject.transform, this.MissileDeleteDelay);
					this.missileInstances[i] = null;
				}
				float num = this.timer / this.missileDuration[i];
				num *= num;
				if (this.moveType == MultiBezierMissilesAction.MoveType.StartToTarget || this.target == null)
				{
					Bezier bezier = new Bezier(this.startPosition, base.variables.skillCaster.transform.right * this.missileOffests[i].offest.x + Vector3.up * this.missileOffests[i].offest.y + base.variables.skillCaster.transform.forward * this.missileOffests[i].offest.z, Vector3.zero, this.targetPosition);
					Vector3 pointAtTime = bezier.GetPointAtTime(Mathf.Clamp01(num));
					gameObject.transform.position = pointAtTime;
					Vector3 forward = this.targetPosition - pointAtTime;
					if (forward.sqrMagnitude > 0.1f)
					{
						Quaternion to = Quaternion.LookRotation(forward);
						gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, to, Time.deltaTime * 10f);
					}
				}
				else
				{
					Vector3 position = base.variables.skillCaster.transform.position;
					position.y += this.YOffset;
					Bezier bezier2 = new Bezier(this.targetPosition, base.variables.skillCaster.transform.right * this.missileOffests[i].offest.x + Vector3.up * this.missileOffests[i].offest.y - base.variables.skillCaster.transform.forward * this.missileOffests[i].offest.z, Vector3.zero, position);
					Vector3 pointAtTime2 = bezier2.GetPointAtTime(Mathf.Clamp01(num));
					gameObject.transform.position = pointAtTime2;
					Vector3 forward2 = this.startPosition - pointAtTime2;
					if (forward2.sqrMagnitude > 0.1f)
					{
						Quaternion to2 = Quaternion.LookRotation(forward2);
						gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, to2, Time.deltaTime * 10f);
					}
				}
			}
		}
	}

	private void OnReachTarget()
	{
		if (this.missileInstances.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.missileInstances.Count; i++)
		{
			GameObject gameObject = this.missileInstances[i];
			if (!(gameObject == null))
			{
				PoolMgr.spawnPool.Despawn(gameObject.transform, this.MissileDeleteDelay);
			}
		}
		this.missileInstances.Clear();
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, this.targetPosition, Quaternion.identity, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		if (base.variables != null && base.variables.skillCaster != null)
		{
			base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, base.variables.skillTarget, base.variables.targetPosition, 0);
		}
	}
}
