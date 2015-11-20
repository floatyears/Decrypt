using System;
using UnityEngine;

public sealed class AITower : AIController
{
	private float timer;

	private ActorController attackTarget;

	private float showMessageCD;

	private int vayIndex;

	public override void Init()
	{
		base.Init();
		this.actorCtrler.SetRoateable(false);
	}

	private void Update()
	{
		if (!this.actorCtrler.Skills[0].IsCooldown)
		{
			return;
		}
		if (this.attackTarget != null && this.actorCtrler.GetDistance2D(this.attackTarget) > this.AttackDistance)
		{
			this.attackTarget = null;
		}
		if ((this.attackTarget == null || this.attackTarget.IsDead) && Time.time > this.timer)
		{
			this.attackTarget = AIController.FindMinDistEnemy(this.actorCtrler, this.AttackDistance);
			this.timer = Time.time + 0.5f;
		}
		if (this.attackTarget == null)
		{
			return;
		}
		this.actorCtrler.TryCastSkill(0, this.attackTarget);
	}

	public override void OnDead()
	{
		base.OnDead();
		Transform transform = base.transform.Find("Dummy001/Dummy006/Dummy003/lizi");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		MemoryGearScene memoryGearScene = Globals.Instance.ActorMgr.MemoryGearScene;
		if (memoryGearScene != null && memoryGearScene.CombatEvent != null)
		{
			memoryGearScene.CombatEvent(EMGEventType.EMGET_TowerDead, this.vayIndex);
		}
	}

	public override void OnDamageTaken()
	{
		base.OnDamageTaken();
		if (Time.time > this.showMessageCD)
		{
			MemoryGearScene memoryGearScene = Globals.Instance.ActorMgr.MemoryGearScene;
			if (memoryGearScene != null && memoryGearScene.CombatEvent != null)
			{
				memoryGearScene.CombatEvent(EMGEventType.EMGET_TowerDamaged, this.vayIndex);
			}
			this.showMessageCD = Time.time + 10f;
		}
	}

	public void SetWayIndex(int index)
	{
		this.vayIndex = index;
	}
}
