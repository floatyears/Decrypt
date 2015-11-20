using System;
using UnityEngine;

public class InstantiateBase : ActionBase
{
	public GameObject prefab;

	public float LifeTime = 60f;

	public float YOffset;

	public float ForwardOffset;

	public bool BaseOnFeet = true;

	protected GameObject go;

	private float timer;

	protected bool checkInterrupt = true;

	protected override void DoAction()
	{
	}

	protected void DoInstantiate(ActorController actor)
	{
		if (actor == null)
		{
			return;
		}
		Vector3 vector = actor.transform.position;
		if (!this.BaseOnFeet)
		{
			vector.y += ((BoxCollider)actor.collider).size.y;
		}
		vector.y += this.YOffset;
		vector += actor.transform.forward * this.ForwardOffset;
		this.DoInstantiate(vector, base.transform.rotation);
	}

	protected void DoInstantiate(Vector3 position, Quaternion rotation)
	{
		Transform transform = PoolMgr.SpawnParticleSystem(this.prefab.transform, position, rotation, base.variables.skillCaster.AttackSpeed);
		this.go = transform.gameObject;
		if (this.go == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Instantiate game object error : {0}", this.prefab.name)
			});
			return;
		}
		if (this.LifeTime <= 0f)
		{
			this.timer = 3.40282347E+38f;
			if (base.variables.action != null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("{0} Lift Time = {1} < 0.0f", base.name, this.LifeTime)
				});
			}
		}
		else
		{
			this.timer = this.LifeTime;
		}
		base.variables.skillCaster.OnSkillStart(base.variables.skillInfo);
	}

	protected void DoInstantiate(ActorController actor, string socketName, bool onlyUseSocketPosition)
	{
		this.DoInstantiate(actor);
		if (this.go != null && !string.IsNullOrEmpty(socketName))
		{
			GameObject gameObject = ObjectUtil.FindChildObject(actor.gameObject, socketName);
			if (gameObject == null)
			{
				return;
			}
			if (onlyUseSocketPosition)
			{
				this.go.transform.position = gameObject.transform.position;
			}
			else
			{
				Transform transform = this.go.transform;
				transform.parent = gameObject.transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				actor.AddPoolSocket(transform);
			}
		}
	}

	protected override void UpdateAction(float elapse)
	{
		if (this.go == null)
		{
			base.Finish();
			return;
		}
		if (this.checkInterrupt && (base.variables == null || base.variables.IsInterrupted()))
		{
			PoolMgr.spawnPool.Despawn(this.go.transform);
			this.go = null;
			return;
		}
		this.timer -= elapse;
		if (this.timer <= 0f)
		{
			PoolMgr.spawnPool.Despawn(this.go.transform);
			this.go = null;
			return;
		}
	}

	protected override void OnDespawned()
	{
		base.OnDespawned();
		if (this.go != null)
		{
			PoolMgr.spawnPool.Despawn(this.go.transform);
			this.go = null;
		}
	}

	protected override void OnPlay()
	{
		base.OnPlay();
		if (this.go == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = this.go.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Play();
		}
		Animator[] componentsInChildren2 = this.go.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].StopPlayback();
		}
	}

	protected override void OnPause()
	{
		base.OnPause();
		if (this.go == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = this.go.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Pause();
		}
		Animator[] componentsInChildren2 = this.go.GetComponentsInChildren<Animator>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].StartPlayback();
		}
	}
}
