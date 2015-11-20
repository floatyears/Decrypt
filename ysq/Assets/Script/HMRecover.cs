using System;
using UnityEngine;

public sealed class HMRecover : MonoBehaviour
{
	public uint HPValue;

	public uint MPValue;

	private float lifeTime;

	private float timer;

	private int status;

	private Vector3 speed = Vector3.one;

	private Vector3 accel = new Vector3(0.3f, 0.3f, 0.3f);

	private Vector3 speedMax = new Vector3(10f, 10f, 10f);

	private float damping = 3f;

	private void OnSpawned()
	{
		this.HPValue = 0u;
		this.MPValue = 0u;
		this.lifeTime = 0f;
		this.timer = 0f;
		this.status = 0;
		this.speed = Vector3.one;
	}

	private void Update()
	{
		if (this.status == 0)
		{
			this.timer += Time.deltaTime;
			if (this.timer >= 0.1f)
			{
				this.lifeTime += this.timer;
				this.timer = 0f;
				if (Globals.Instance.ActorMgr.PlayerCtrler != null)
				{
					if (Globals.Instance.ActorMgr.PlayerCtrler.ActorCtrler.AiCtrler.EnableAI)
					{
						this.status = 1;
						this.lifeTime = 0f;
						this.timer = Time.time;
					}
					else if (CombatHelper.DistanceSquared2D(Globals.Instance.ActorMgr.PlayerCtrler.transform.position, base.transform.position) < 9f)
					{
						this.status = 1;
						this.lifeTime = 0f;
						this.timer = Time.time;
					}
				}
				if (this.lifeTime > 30f)
				{
					PoolMgr.Despawn(base.gameObject.transform);
				}
			}
		}
		else if (Globals.Instance.ActorMgr.PlayerCtrler != null)
		{
			this.speed = this.accel * Mathf.Pow(Time.time - this.timer, 4f);
			if (this.speed.x > this.speedMax.x)
			{
				this.speed = this.speedMax;
			}
			Vector3 position = Globals.Instance.ActorMgr.PlayerCtrler.transform.position;
			position.y += 0.5f;
			Vector3 vector = position - base.transform.position;
			if (vector != Vector3.zero)
			{
				Quaternion to = Quaternion.LookRotation(vector);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, Time.deltaTime * this.damping);
			}
			Vector3 vector2 = this.speed * Time.deltaTime;
			base.transform.position += vector.normalized * vector2.x;
			base.transform.position += vector.normalized * vector2.y;
			base.transform.position += vector.normalized * vector2.z;
			if (vector.magnitude < vector2.magnitude)
			{
				this.OnTouchTarget();
			}
		}
		else
		{
			this.OnTouchTarget();
		}
	}

	private void OnTouchTarget()
	{
		Globals.Instance.ActorMgr.AddHPMP(this.HPValue, this.MPValue);
		PoolMgr.Despawn(base.gameObject.transform);
	}
}
