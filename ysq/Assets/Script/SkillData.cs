using Att;
using System;
using UnityEngine;

public class SkillData
{
	public const int AUTO_ATTACK_INDEX = 0;

	public const int AUTO_ATTACK_ID = 16;

	public SkillInfo Info;

	public float Cooldown;

	public int Rate;

	public int Count;

	public long Damage;

	public long Heal;

	public long HighDamage;

	public int Effect;

	public bool IsCooldown
	{
		get
		{
			return this.Cooldown <= 0f;
		}
	}

	public float CoolPercent
	{
		get
		{
			float value = (this.Info.CoolDown - this.Cooldown) / this.Info.CoolDown;
			return Mathf.Clamp(value, 0f, 1f);
		}
	}

	public int RemainCoolDownTime
	{
		get
		{
			return Mathf.CeilToInt(Mathf.Clamp(this.Cooldown, 0f, this.Info.CoolDown));
		}
	}

	public static bool IsAutoAttack(int index)
	{
		return 0 == index;
	}
}
