using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/MultiTargetExplodeAction")]
public class MultiTargetExplodeAction : ActionBase
{
	public class TargetInfo
	{
		public GameObject explode;

		public ActorController actor;

		public float timer;

		public bool damage;
	}

	public GameObject explodePrefab;

	public float YOffset;

	public float explodeDelay = 1f;

	public int TickCount = 5;

	public float TickInterval = 1f;

	private List<MultiTargetExplodeAction.TargetInfo> targets = new List<MultiTargetExplodeAction.TargetInfo>();

	private float tickTimer;

	private int index;

	private void Awake()
	{
		this.interrupt = true;
	}

	protected override void DoAction()
	{
		this.index = 0;
		if (this.index >= this.TickCount)
		{
			base.Finish();
			return;
		}
		this.tickTimer = this.TickInterval / base.variables.skillCaster.AttackSpeed;
	}

	protected override void UpdateAction(float elapse)
	{
		if (base.variables == null || base.variables.IsInterrupted())
		{
			base.Finish();
			return;
		}
		this.tickTimer -= elapse;
		if (this.tickTimer <= 0f && this.index < this.TickCount)
		{
			this.index++;
			this.CastSkill();
			this.tickTimer = this.TickInterval / base.variables.skillCaster.AttackSpeed;
		}
		if (!this.UpdateTarget(elapse) && this.index >= this.TickCount)
		{
			base.Finish();
			return;
		}
	}

	private void CastSkill()
	{
		EEffectTargetType effectTargetType = (EEffectTargetType)base.variables.skillInfo.EffectTargetType;
		if (effectTargetType != EEffectTargetType.EETT_AllEnemyAroundCaster)
		{
			if (effectTargetType != EEffectTargetType.EETT_AllFriendAroundCaster)
			{
				global::Debug.LogError(new object[]
				{
					string.Concat(new object[]
					{
						"Not support SkillID = [",
						base.variables.skillInfo.ID,
						"] invalid EffectTargetType = [",
						base.variables.skillInfo.EffectTargetType,
						"]"
					})
				});
				base.Finish();
				return;
			}
			List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorController actorController = actors[i];
				if (actorController && !actorController.IsDead && base.variables.skillCaster.IsFriendlyTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
					{
						this.AddTarget(new MultiTargetExplodeAction.TargetInfo
						{
							explode = UnityEngine.Object.Instantiate(this.explodePrefab, actorController.transform.position, actorController.transform.rotation) as GameObject,
							actor = actorController,
							timer = this.explodeDelay
						});
					}
				}
			}
		}
		else
		{
			List<ActorController> actors2 = Globals.Instance.ActorMgr.Actors;
			for (int j = 0; j < actors2.Count; j++)
			{
				ActorController actorController = actors2[j];
				if (actorController && !actorController.IsDead && base.variables.skillCaster.IsHostileTo(actorController))
				{
					if (CombatHelper.DistanceSquared2D(base.variables.skillCaster.transform.position, actorController.transform.position) <= (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()) * (base.variables.skillInfo.Radius + actorController.GetBoundsRadius()))
					{
						this.AddTarget(new MultiTargetExplodeAction.TargetInfo
						{
							explode = UnityEngine.Object.Instantiate(this.explodePrefab, actorController.transform.position, actorController.transform.rotation) as GameObject,
							actor = actorController,
							timer = this.explodeDelay
						});
					}
				}
			}
		}
	}

	private void AddTarget(MultiTargetExplodeAction.TargetInfo targetInfo)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] == null)
			{
				this.targets[i] = targetInfo;
				return;
			}
		}
		this.targets.Add(targetInfo);
	}

	private bool UpdateTarget(float elapse)
	{
		bool result = false;
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] != null)
			{
				this.targets[i].timer -= elapse;
				if (this.targets[i].timer < 0f)
				{
					if (this.targets[i].damage)
					{
						if (this.targets[i].explode != null)
						{
							UnityEngine.Object.Destroy(this.targets[i].explode);
							this.targets[i].explode = null;
						}
						this.targets[i] = null;
						goto IL_11B;
					}
					base.variables.skillCaster.DoEffectOnTarget(base.variables.skillInfo, this.targets[i].actor, 0);
					this.targets[i].timer = 1f;
					this.targets[i].damage = true;
				}
				result = true;
			}
			IL_11B:;
		}
		return result;
	}

	private new void OnDespawned()
	{
		base.OnDespawned();
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i] != null && this.targets[i].explode != null)
			{
				UnityEngine.Object.Destroy(this.targets[i].explode);
				this.targets[i].explode = null;
			}
		}
		this.targets.Clear();
	}
}
