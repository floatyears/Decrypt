using Att;
using Proto;
using System;

public class AchievementDataEx : BaseData
{
	public AchievementData Data;

	public AchievementInfo Info;

	public bool IsComplete()
	{
		if (this.Data == null || this.Info == null)
		{
			return false;
		}
		if (this.Info.ConditionType == 16)
		{
			return !Globals.Instance.Player.IsCardExpire() && !Globals.Instance.Player.IsTodayCardDiamondTaken();
		}
		if (this.Info.ConditionType == 17)
		{
			return Globals.Instance.Player.IsBuySuperCard() && !Globals.Instance.Player.IsTodaySuperCardDiamondTaken();
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)this.Info.Level))
		{
			return false;
		}
		int value = this.GetValue();
		if (value == 0)
		{
			return false;
		}
		if (this.Info.ConditionType == 39)
		{
			return value <= this.Info.Value;
		}
		return value >= this.Info.Value;
	}

	public bool IsShowUI()
	{
		return this.Info != null && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)this.Info.Level);
	}

	public int GetValue()
	{
		if (this.Info.Daily && this.Data.CoolDown != 0 && Globals.Instance.Player.GetTimeStamp() >= this.Data.CoolDown)
		{
			return 0;
		}
		return this.Data.Value;
	}

	public bool IsCard()
	{
		return this.Info.ConditionType == 16 || this.Info.ConditionType == 17;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
