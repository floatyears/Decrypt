using System;
using System.Collections.Generic;

public sealed class ThreatMgr
{
	private ActorController actorCtrler;

	private List<ThreatData> threatList = new List<ThreatData>();

	public ActorController TargetActor;

	private bool updateFlag;

	public bool IsEmpty
	{
		get
		{
			return this.threatList.Count == 0;
		}
	}

	public ThreatMgr(ActorController actor)
	{
		this.actorCtrler = actor;
	}

	public void AddThreat(ActorController enemyActor, float threat)
	{
		if (enemyActor == null || enemyActor == this.actorCtrler || enemyActor.IsDead || this.actorCtrler.IsDead || !this.actorCtrler.IsHostileTo(enemyActor))
		{
			return;
		}
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (this.threatList[i].EnemyActor == enemyActor)
			{
				this.threatList[i].Threat += threat;
				return;
			}
		}
		ThreatData threatData = new ThreatData();
		threatData.EnemyActor = enemyActor;
		threatData.Threat = threat;
		this.threatList.Add(threatData);
		this.updateFlag = true;
	}

	public float GetThreat(ActorController enemyActor)
	{
		if (enemyActor == null)
		{
			return 0f;
		}
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (this.threatList[i].EnemyActor == enemyActor)
			{
				return this.threatList[i].Threat;
			}
		}
		return 0f;
	}

	public void Clear()
	{
		this.threatList.Clear();
		this.updateFlag = false;
		this.TargetActor = null;
	}

	private static bool IsDeleted(ThreatData data)
	{
		return data.EnemyActor == null || data.EnemyActor.IsDead;
	}

	public static int Predicate(ThreatData left, ThreatData right)
	{
		if (left.Threat > right.Threat)
		{
			return 1;
		}
		return 0;
	}

	public void OrderList()
	{
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (this.threatList[i].EnemyActor != null && !this.actorCtrler.IsHostileTo(this.threatList[i].EnemyActor))
			{
				this.threatList[i].EnemyActor = null;
			}
		}
		this.threatList.RemoveAll(new Predicate<ThreatData>(ThreatMgr.IsDeleted));
		if (this.updateFlag && this.threatList.Count > 1)
		{
			this.threatList.Sort(new Comparison<ThreatData>(ThreatMgr.Predicate));
		}
		this.updateFlag = false;
	}

	public void SetAttackTarget(ActorController enemyActor)
	{
		this.TargetActor = enemyActor;
	}

	public ActorController GetAttackTarget()
	{
		if (this.threatList.Count == 0)
		{
			this.TargetActor = null;
			return null;
		}
		this.OrderList();
		float threat = this.GetThreat(this.TargetActor);
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (!this.threatList[i].EnemyActor.IsImmunity || this.threatList[i].EnemyActor.ActorType != ActorController.EActorType.EMonster)
			{
				if (this.TargetActor == null || this.threatList[i].EnemyActor == this.TargetActor)
				{
					this.TargetActor = this.threatList[i].EnemyActor;
					return this.TargetActor;
				}
				if (this.threatList[i].EnemyActor.ActorType == ActorController.EActorType.EPlayer)
				{
					if (this.threatList[i].Threat > threat * 1.5f)
					{
						this.TargetActor = this.threatList[i].EnemyActor;
						return this.TargetActor;
					}
				}
				else if (this.threatList[i].Threat > threat * 1.2f)
				{
					this.TargetActor = this.threatList[i].EnemyActor;
					return this.TargetActor;
				}
			}
		}
		this.TargetActor = null;
		return null;
	}
}
