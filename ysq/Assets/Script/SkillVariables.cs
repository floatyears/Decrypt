using Att;
using System;
using UnityEngine;

public class SkillVariables
{
	public SkillInfo skillInfo;

	public ActorController skillCaster;

	public ActorController skillTarget;

	public Vector3 targetPosition = Vector3.zero;

	public int skillSerialID;

	public Transform action;

	private int actionIndex;

	private int actionFlag;

	public int GenerateActionIndex()
	{
		this.actionIndex++;
		if (this.actionIndex == 0 || this.actionIndex > 30)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("actionIndex = {0} invalid", this.actionIndex)
			});
			return 0;
		}
		this.actionFlag |= 1 << this.actionIndex;
		return this.actionIndex;
	}

	public void ActionDone(int index)
	{
		if (index == 0 || index > 30)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("index = {0} invalid", index)
			});
			return;
		}
		this.actionFlag &= ~(1 << index);
	}

	public bool IsDone()
	{
		return 0 == this.actionFlag;
	}

	public bool IsInterrupted()
	{
		return this.skillCaster == null || (this.skillSerialID != 0 && this.skillSerialID != this.skillCaster.SkillSerialID);
	}

	public bool CheckInterrupt()
	{
		if ((this.skillInfo.CastTargetType == 1 || this.skillInfo.CastTargetType == 2) && this.skillInfo.EffectTargetType == 0 && (this.skillTarget == null || this.skillTarget.IsDead))
		{
			if (this.skillCaster.InterruptSkill(this.skillSerialID))
			{
				this.skillCaster.AnimationCtrler.StopAnimation();
			}
			return true;
		}
		return false;
	}
}
