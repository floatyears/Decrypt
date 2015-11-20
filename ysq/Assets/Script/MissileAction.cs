using System;
using UnityEngine;

[AddComponentMenu("Game/Action/Missile Action")]
public class MissileAction : ActionBase
{
	public GameObject MissilePrefab;

	public float MissileDeleteDelay = 0.2f;

	public float YOffset;

	public float ForwardOffset;

	public float RightOffset;

	public GameObject explodePrefab;

	public float explodeLifeTime = 1f;

	public int Index;

	protected GameObject missile;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
	}

	protected void CreateMissile(Transform startTrans)
	{
		if (this.MissilePrefab != null)
		{
			Vector3 vector = startTrans.position;
			vector.y += this.YOffset;
			vector += startTrans.forward * this.ForwardOffset;
			vector += startTrans.right * this.RightOffset;
			Transform transform = PoolMgr.SpawnParticleSystem(this.MissilePrefab.transform, vector, base.transform.rotation, 1f);
			this.missile = transform.gameObject;
			if (this.missile == null)
			{
				global::Debug.LogError(new object[]
				{
					"Instantiate MissilePrefab object error!"
				});
				return;
			}
		}
	}

	protected void CreateMissile()
	{
		this.CreateMissile(base.transform);
	}

	protected virtual void OnTouchTarget(Vector3 position)
	{
		if (this.missile != null)
		{
			PoolMgr.spawnPool.Despawn(this.missile.transform, this.MissileDeleteDelay);
			this.missile = null;
		}
		if (this.explodePrefab != null)
		{
			Transform instance = PoolMgr.SpawnParticleSystem(this.explodePrefab.transform, position, Quaternion.identity, 1f);
			PoolMgr.spawnPool.Despawn(instance, this.explodeLifeTime);
		}
		if (base.variables != null && base.variables.skillCaster != null)
		{
			base.variables.skillCaster.OnMissileHit(base.variables.skillInfo, base.variables.skillTarget, base.variables.targetPosition, this.Index);
		}
	}

	protected new void OnDespawned()
	{
		base.OnDespawned();
		if (this.missile != null)
		{
			PoolMgr.spawnPool.Despawn(this.missile.transform);
			this.missile = null;
		}
	}
}
