using Att;
using Proto;
using System;

public sealed class AAItemDataEx : BaseData
{
	public ActivityAchievementData AA
	{
		get;
		private set;
	}

	public AAItemData AAData
	{
		get;
		private set;
	}

	public AAItemDataEx(ActivityAchievementData aa, AAItemData data)
	{
		this.AA = aa;
		this.AAData = data;
	}

	public bool IsComplete()
	{
		return this.AAData.CurValue >= this.AAData.Value;
	}

	public bool IsTakeReward()
	{
		return this.AAData.Flag;
	}

	public string GetTitle()
	{
		if (this.AAData.Value != 0)
		{
			return Singleton<StringManager>.Instance.GetString(string.Format("EACT_{0}", this.AAData.Type), new object[]
			{
				Tools.GetValueText((EAchievementConditionType)this.AAData.Type, this.AAData.Value)
			});
		}
		return string.Empty;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
