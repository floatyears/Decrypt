using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/SholayAction")]
public sealed class SholayAction : ActionBase
{
	public GameObject MissilePrefab;

	public float MissileDeleteDelay = 0.1f;

	public float YOffset = 0.5f;

	public float ForwardOffset = 0.5f;

	public List<float> delayMissiles = new List<float>();

	public float MissileSpeed = 5f;

	public float AroundSpeed = 180f;

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	private List<GameObject> Missiles;

	private List<ActorController> MissileTargets;

	private List<float> MissileLifeTimers;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.MissileLifeTimers = new List<float>();
		this.Missiles = new List<GameObject>();
		this.MissileTargets = new List<ActorController>();
		int count = this.delayMissiles.Count;
		Vector3 vector = base.variables.skillCaster.transform.position;
		vector.y += this.YOffset;
		vector += base.variables.skillCaster.transform.forward * this.ForwardOffset;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.MissilePrefab, vector, Quaternion.identity) as GameObject;
			gameObject.transform.RotateAround(base.variables.skillCaster.transform.position, Vector3.up, (float)(360 / count * i));
			this.Missiles.Add(gameObject);
			this.MissileTargets.Add(null);
			this.MissileLifeTimers.Add(0f);
		}
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null)
		{
			base.Finish();
			return;
		}
		int num = 0;
		for (int i = 0; i < this.MissileLifeTimers.Count; i++)
		{
			List<float> missileLifeTimers;
			List<float> expr_21 = missileLifeTimers = this.MissileLifeTimers;
			int index;
			int expr_25 = index = i;
			float num2 = missileLifeTimers[index];
			expr_21[expr_25] = num2 + Time.deltaTime;
			GameObject gameObject = this.Missiles[i];
			if (!(gameObject == null))
			{
				float num3 = this.MissileLifeTimers[i] - this.delayMissiles[i];
				if (num3 > 0f)
				{
					if (this.MissileTargets[i] == null)
					{
						this.MissileTargets[i] = this.FindMissleTarget(gameObject);
					}
					if (this.MissileTargets[i] == null)
					{
						UnityEngine.Object.Destroy(gameObject, this.MissileDeleteDelay);
						this.Missiles[i] = null;
					}
					else
					{
						num++;
						Vector3 position = this.MissileTargets[i].transform.position;
						position.y += this.YOffset;
						gameObject.transform.LookAt(position);
						float num4 = this.MissileSpeed * Time.deltaTime;
						float num5 = Vector3.Distance(gameObject.transform.position, position);
						if (num5 < num4)
						{
							gameObject.transform.position = position;
							UnityEngine.Object.Destroy(gameObject, this.MissileDeleteDelay);
							this.Missiles[i] = null;
							if (this.explodePrefab != null)
							{
								Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, position, Quaternion.identity, 1f);
								PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
							}
							if (base.variables != null && base.variables.skillCaster != null)
							{
								base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, this.MissileTargets[i], base.variables.targetPosition, i);
							}
							this.MissileTargets[i] = null;
						}
						else
						{
							gameObject.transform.Translate(Vector3.forward * num4);
						}
					}
				}
				else
				{
					num++;
					Vector3 vector = base.variables.skillCaster.transform.position;
					vector.y += this.YOffset;
					vector += base.variables.skillCaster.transform.forward * this.ForwardOffset;
					gameObject.transform.position = vector;
					gameObject.transform.RotateAround(base.variables.skillCaster.transform.position, Vector3.up, this.AroundSpeed * this.MissileLifeTimers[i] + (float)(360 / this.MissileLifeTimers.Count * i));
				}
			}
		}
		if (num == 0)
		{
			base.Finish();
		}
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		if (this.Missiles != null)
		{
			for (int i = 0; i < this.Missiles.Count; i++)
			{
				GameObject gameObject = this.Missiles[i];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			this.Missiles.Clear();
			this.Missiles = null;
		}
		if (this.MissileTargets != null)
		{
			this.MissileTargets.Clear();
			this.MissileTargets = null;
		}
	}

	private ActorController FindMissleTarget(GameObject missile)
	{
		ActorController result = null;
		float num = 3.40282347E+38f;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorController actorController = actors[i];
			if (actorController && !(actorController == base.variables.skillCaster) && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
			{
				float num2 = CombatHelper.DistanceSquared2D(missile.transform.position, actorController.transform.position);
				if (base.variables.skillInfo == null || num2 <= base.variables.skillInfo.Radius * base.variables.skillInfo.Radius)
				{
					if (num2 < num)
					{
						result = actorController;
						num = num2;
					}
				}
			}
		}
		return result;
	}

	protected override void OnPlay()
	{
		base.OnPlay();
		if (this.Missiles == null)
		{
			return;
		}
		for (int i = 0; i < this.Missiles.Count; i++)
		{
			GameObject gameObject = this.Missiles[i];
			if (!(gameObject == null))
			{
				ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].Play();
				}
				Animator[] componentsInChildren2 = gameObject.GetComponentsInChildren<Animator>();
				for (int k = 0; k < componentsInChildren2.Length; k++)
				{
					componentsInChildren2[k].StopPlayback();
				}
			}
		}
	}

	protected override void OnPause()
	{
		base.OnPause();
		if (this.Missiles == null)
		{
			return;
		}
		for (int i = 0; i < this.Missiles.Count; i++)
		{
			GameObject gameObject = this.Missiles[i];
			if (!(gameObject == null))
			{
				ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].Pause();
				}
				Animator[] componentsInChildren2 = gameObject.GetComponentsInChildren<Animator>();
				for (int k = 0; k < componentsInChildren2.Length; k++)
				{
					componentsInChildren2[k].StartPlayback();
				}
			}
		}
	}
}
