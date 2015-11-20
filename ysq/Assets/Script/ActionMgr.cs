using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionMgr : Singleton<ActionMgr>
{
	private List<SkillVariables> skillInsts = new List<SkillVariables>();

	private float timer;

	public void PlayCastAction(ActorController caster, SkillInfo info, ActorController target, Vector3 targetPos)
	{
		if (caster == null || info == null)
		{
			global::Debug.LogError(new object[]
			{
				"caster == null || info == null"
			});
			return;
		}
		if (string.IsNullOrEmpty(info.CastAction))
		{
			global::Debug.LogError(new object[]
			{
				string.Format("SkillID = [{0}] invalid CastAction", info.ID)
			});
			return;
		}
		Transform transform = PoolMgr.Spawn(info.CastAction, caster.transform.position, caster.transform.rotation);
		if (transform == null)
		{
			global::Debug.Log(new object[]
			{
				string.Format("PoolMgr.Spawn error, actionName = {0}", info.CastAction)
			});
			return;
		}
		SkillVariables skillVariables = new SkillVariables();
		skillVariables.skillCaster = caster;
		skillVariables.skillInfo = info;
		skillVariables.skillTarget = target;
		skillVariables.targetPosition = targetPos;
		skillVariables.action = transform;
		if (info.CastType == 0)
		{
			skillVariables.skillSerialID = caster.GenerateSkillSerialID();
		}
		transform.SendMessage("OnInit", skillVariables, SendMessageOptions.DontRequireReceiver);
		this.skillInsts.Add(skillVariables);
	}

	public void PlayHitAction(ActorController caster, SkillInfo info, ActorController target)
	{
		if (target == null || info == null)
		{
			global::Debug.LogError(new object[]
			{
				"target == null || info == null"
			});
			return;
		}
		for (int i = 0; i < info.HitAction.Count; i++)
		{
			if (!string.IsNullOrEmpty(info.HitAction[i]))
			{
				Transform transform = PoolMgr.Spawn(info.HitAction[i], target.transform.position, target.transform.rotation);
				if (transform != null)
				{
					SkillVariables skillVariables = new SkillVariables();
					skillVariables.skillCaster = target;
					skillVariables.skillInfo = info;
					skillVariables.skillTarget = caster;
					skillVariables.action = transform;
					transform.SendMessage("OnInit", skillVariables, SendMessageOptions.DontRequireReceiver);
					this.skillInsts.Add(skillVariables);
				}
			}
		}
	}

	public void PlayAction(ActorController caster, string actionName, ActorController target)
	{
		if (caster == null || string.IsNullOrEmpty(actionName))
		{
			return;
		}
		Transform transform = PoolMgr.Spawn(actionName, caster.transform.position, caster.transform.rotation);
		if (transform != null)
		{
			SkillVariables skillVariables = new SkillVariables();
			skillVariables.skillCaster = caster;
			skillVariables.skillTarget = target;
			skillVariables.action = transform;
			transform.SendMessage("OnInit", skillVariables, SendMessageOptions.DontRequireReceiver);
			this.skillInsts.Add(skillVariables);
		}
	}

	public void Update(float elapse)
	{
		this.timer += elapse;
		if (this.timer < 0.1f)
		{
			return;
		}
		this.timer = 0f;
		if (this.skillInsts.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.skillInsts.Count; i++)
		{
			if (this.skillInsts[i] != null && this.skillInsts[i].IsDone() && this.skillInsts[i].action != null)
			{
				PoolMgr.Despawn(this.skillInsts[i].action);
				this.skillInsts[i].action = null;
			}
		}
		this.skillInsts.RemoveAll(new Predicate<SkillVariables>(ActionMgr.IsDeletedSkill));
	}

	private static bool IsDeletedSkill(SkillVariables skill)
	{
		return skill == null || skill.action == null;
	}

	public void Pause(bool value, ActorController actor = null)
	{
		for (int i = 0; i < this.skillInsts.Count; i++)
		{
			if (this.skillInsts[i] != null && this.skillInsts[i].action != null && (actor == null || this.skillInsts[i].skillCaster == actor))
			{
				if (value)
				{
					this.skillInsts[i].action.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					this.skillInsts[i].action.SendMessage("OnPlay", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
