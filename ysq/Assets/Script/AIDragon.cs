using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AIDragon : AIController
{
	private Vector3 skillPositions = Vector3.zero;

	private float skillRotations;

	private bool initSkillCD;

	private float attackCamInitTime;

	public override void Init()
	{
		base.Init();
		this.actorCtrler.AddSkill(5, 5047, true);
		this.actorCtrler.AddSkill(6, 5048, true);
		this.skillPositions = new Vector3(6.511289f, -2.766201f, -1.545421f);
		this.skillRotations = 46.93544f;
	}

	private void Update()
	{
		if (Globals.Instance.CameraMgr.worldBossAttackPlay && Time.time - this.attackCamInitTime >= 5f)
		{
			Globals.Instance.CameraMgr.worldBossAttackPlay = false;
		}
		if (this.actorCtrler == null || this.actorCtrler.IsDead || this.Locked)
		{
			return;
		}
		if (!this.autoAttack || this.actorCtrler.LockSkillIndex != -1 || this.actorCtrler.SkillCastCache != -1)
		{
			return;
		}
		float num = 8f;
		ActorController actorController = null;
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			ActorController actorController2 = actors[i];
			if (actorController2 && !actorController2.IsDead && this.actorCtrler.IsHostileTo(actorController2))
			{
				float distance2D = this.actorCtrler.GetDistance2D(actorController2);
				if (distance2D < num)
				{
					num = distance2D;
					actorController = actorController2;
				}
			}
		}
		if (actorController == null)
		{
			return;
		}
		if (actors[0] != null && actors[0].IsDead && this.initSkillCD)
		{
			this.initSkillCD = false;
		}
		if (!this.initSkillCD)
		{
			this.initSkillCD = true;
			this.actorCtrler.InitMonsterSkillCD();
		}
		int num2 = 1;
		SkillData skillData = this.actorCtrler.Skills[num2];
		if (skillData != null && skillData.IsCooldown)
		{
			ActorController actorController2 = Globals.Instance.ActorMgr.GetActor(0);
			if (actorController2 != null && !actorController2.IsDead)
			{
				ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(num2, actorController2);
				if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
				{
					Globals.Instance.CameraMgr.worldBossAttackPlay = true;
					this.attackCamInitTime = Time.time;
					return;
				}
			}
		}
		num2 = 2;
		skillData = this.actorCtrler.Skills[num2];
		if (skillData != null && skillData.IsCooldown)
		{
			this.actorCtrler.SkillValue = this.skillRotations;
			ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(num2, this.skillPositions);
			if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
			{
				Globals.Instance.CameraMgr.worldBossAttackPlay = true;
				this.attackCamInitTime = Time.time;
				return;
			}
		}
		num2 = 3;
		skillData = this.actorCtrler.Skills[num2];
		if (skillData != null && skillData.IsCooldown)
		{
			ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(num2, actorController);
			if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
			{
				Globals.Instance.CameraMgr.worldBossAttackPlay = true;
				this.attackCamInitTime = Time.time;
				return;
			}
		}
		if (num > 4.5f)
		{
			return;
		}
		num2 = 0;
		skillData = this.actorCtrler.Skills[num2];
		if (skillData != null && skillData.IsCooldown)
		{
			ECastSkillResult eCastSkillResult = this.actorCtrler.TryCastSkill(num2, actorController);
			if (eCastSkillResult == ECastSkillResult.ECSR_Sucess || eCastSkillResult == ECastSkillResult.ECSR_Cache)
			{
				return;
			}
		}
		if (UtilFunc.RangeRandom(0, 10000) > 5000)
		{
			num2 = 5;
			skillData = this.actorCtrler.Skills[num2];
			if (skillData != null && skillData.IsCooldown)
			{
				this.actorCtrler.TryCastSkill(num2, actorController);
			}
		}
		else
		{
			num2 = 6;
			skillData = this.actorCtrler.Skills[num2];
			if (skillData != null && skillData.IsCooldown)
			{
				this.actorCtrler.TryCastSkill(num2, actorController);
			}
		}
	}

	public void SetInitSkillCD(bool value)
	{
		this.initSkillCD = value;
	}
}
