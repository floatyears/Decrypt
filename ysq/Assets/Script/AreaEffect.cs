using Att;
using System;
using UnityEngine;

public sealed class AreaEffect : MonoBehaviour
{
	public ActorController caster;

	public int OrignalSkillIndex = -1;

	public SkillInfo skillInfo;

	public float TickInterval = 0.5f;

	public int TickCount = 6;

	public bool FollowCaster;

	public float MoveForwardSpeed;

	public float tickTimer;

	public bool Pool;

	public float MaxDuration;

	private int index;

	private float lifeTimer;

	private bool destroyFlag;

	private void OnSpawned()
	{
		this.FollowCaster = false;
		this.MoveForwardSpeed = 0f;
		this.tickTimer = 0f;
		this.index = 0;
		this.lifeTimer = 0f;
		this.destroyFlag = false;
		this.OrignalSkillIndex = -1;
	}

	private void Update()
	{
		if (this.destroyFlag)
		{
			return;
		}
		if (this.FollowCaster)
		{
			base.transform.position = this.caster.transform.position;
		}
		else if (this.MoveForwardSpeed > 0f)
		{
			base.transform.position += base.transform.forward * this.MoveForwardSpeed * Time.deltaTime;
		}
		this.tickTimer += Time.deltaTime;
		this.lifeTimer += Time.deltaTime;
		if (this.tickTimer >= this.TickInterval && this.index < this.TickCount)
		{
			this.tickTimer -= this.TickInterval;
			if (!(this.caster != null) || this.caster.IsDead)
			{
				if (this.Pool)
				{
					PoolMgr.Despawn(base.transform);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				this.destroyFlag = true;
				return;
			}
			if (this.skillInfo != null)
			{
				this.caster.SkillCountIndex = this.OrignalSkillIndex;
				this.caster.OnSkillCast(this.skillInfo, null, base.transform.position, this.index);
				this.caster.SkillCountIndex = -1;
			}
			this.index++;
		}
		if (this.index >= this.TickCount && this.lifeTimer >= this.MaxDuration)
		{
			if (this.MoveForwardSpeed > 0f || this.MaxDuration > this.TickInterval * (float)this.TickCount)
			{
				if (this.Pool)
				{
					PoolMgr.Despawn(base.transform);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			else if (this.Pool)
			{
				PoolMgr.Despawn(base.transform, 2f);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject, 2f);
			}
			this.destroyFlag = true;
		}
	}
}
