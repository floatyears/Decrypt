using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIGear : AIController
{
	public class EffectInst
	{
		public Transform trans;

		public float timer;
	}

	private List<AIGear.EffectInst> effectLists = new List<AIGear.EffectInst>();

	private float timer;

	private List<ActorController> actors;

	public override void Init()
	{
		base.Init();
		this.actors = Globals.Instance.ActorMgr.Actors;
		PoolMgr.CreatePrefabPool("Skill/com/st_075", 2, 5);
		PoolMgr.CreatePrefabPool("Skill/misc_006", 2, 5);
	}

	private void Update()
	{
		this.timer += Time.deltaTime;
		if (this.timer > 0.3f)
		{
			this.timer = 0f;
			if (this.actors == null)
			{
				return;
			}
			int i = 5;
			while (i < this.actors.Count)
			{
				if (this.actors[i] == null || this.actors[i].FactionType == ActorController.EFactionType.EBlue)
				{
					i++;
				}
				else
				{
					float distance2D = this.actors[i].GetDistance2D(base.transform.position);
					if (distance2D < 2.5f)
					{
						this.actors[i].RemoveAllBuff();
						this.PlayDisapperEffect(this.actors[i].transform.position, 1.2f);
						UnityEngine.Object.Destroy(this.actors[i].gameObject);
						Globals.Instance.ActorMgr.MemoryGearScene.AddDamageCount((!this.actors[i].IsBoss) ? 1 : 5);
						this.actors.RemoveAt(i);
						this.actorCtrler.PlayAction("Skill/misc_006", null);
					}
					else
					{
						i++;
					}
				}
			}
		}
		this.UpdateDisapperEffect();
	}

	private void PlayDisapperEffect(Vector3 pos, float timer)
	{
		AIGear.EffectInst effectInst = new AIGear.EffectInst();
		effectInst.timer = Time.time + timer;
		effectInst.trans = PoolMgr.Spawn("Skill/com/st_075", pos, Quaternion.identity);
		this.effectLists.Add(effectInst);
	}

	private void UpdateDisapperEffect()
	{
		int i = 0;
		while (i < this.effectLists.Count)
		{
			if (Time.time >= this.effectLists[i].timer)
			{
				PoolMgr.Despawn(this.effectLists[i].trans);
				this.effectLists.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}
}
