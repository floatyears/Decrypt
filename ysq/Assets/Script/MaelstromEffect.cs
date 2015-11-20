using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MaelstromEffect : MonoBehaviour
{
	public ActorController caster;

	public float radius;

	public int BuffID;

	public float MaxDuration = 6f;

	public float Speed = 1.5f;

	private float tickTimer;

	private bool deleted;

	private float delayTimer;

	public int DelayTime
	{
		set
		{
			this.delayTimer = (float)value / 100f;
		}
	}

	private void OnSpawned()
	{
		this.tickTimer = 0f;
		this.deleted = false;
		this.delayTimer = 0f;
	}

	private void Update()
	{
		if (this.deleted)
		{
			return;
		}
		if (this.delayTimer > 0f)
		{
			this.delayTimer -= Time.deltaTime;
			if (this.delayTimer > 0f)
			{
				return;
			}
		}
		this.tickTimer += Time.deltaTime;
		if (this.tickTimer >= this.MaxDuration)
		{
			this.DestroyEffect();
			return;
		}
		if (this.caster != null && !this.caster.IsDead)
		{
			List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorController actorController = actors[i];
				if (actorController && !actorController.IsDead)
				{
					if (!this.caster.IsHostileTo(actorController))
					{
						if (this.BuffID != 0)
						{
							actorController.RemoveBuff(this.BuffID, this.caster);
						}
					}
					else if (CombatHelper.DistanceSquared2D(base.transform.position, actorController.transform.position) > (this.radius + actorController.GetBoundsRadius()) * (this.radius + actorController.GetBoundsRadius()))
					{
						if (this.BuffID != 0)
						{
							actorController.RemoveBuff(this.BuffID, this.caster);
						}
					}
					else
					{
						if (this.BuffID != 0)
						{
							actorController.AddBuff(this.BuffID, this.caster);
						}
						Vector3 vector = base.transform.position - actorController.transform.position;
						if (vector.magnitude > 0.5f)
						{
							float d = this.Speed * Time.deltaTime;
							actorController.transform.position += vector.normalized * d;
							actorController.transform.position += vector.normalized * d;
							actorController.transform.position += vector.normalized * d;
						}
					}
				}
			}
		}
		else
		{
			this.DestroyEffect();
		}
	}

	private void DestroyEffect()
	{
		this.deleted = true;
		if (this.BuffID != 0)
		{
			List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorController actorController = actors[i];
				if (actorController && !actorController.IsDead)
				{
					actorController.RemoveBuff(this.BuffID, this.caster);
				}
			}
		}
	}
}
