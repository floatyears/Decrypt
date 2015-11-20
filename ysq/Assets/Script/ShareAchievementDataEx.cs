using Att;
using Proto;
using System;

public class ShareAchievementDataEx
{
	public ShareAchievementData Data;

	public ShareAchievementInfo Info;

	public bool IsComplete()
	{
		if (this.Data.Value == 0)
		{
			return false;
		}
		if (this.Info.ConditionType == 39)
		{
			return this.Data.Value <= this.Info.Value;
		}
		return this.Data.Value >= this.Info.Value;
	}
}
