using Att;
using System;

public sealed class FundRewardData : BaseData
{
	public MiscInfo Info
	{
		get;
		private set;
	}

	public bool IsWelfare
	{
		get;
		private set;
	}

	public FundRewardData(MiscInfo info, bool isWelfare)
	{
		this.Info = info;
		this.IsWelfare = isWelfare;
	}

	public bool IsTakeReward()
	{
		if (this.Info == null)
		{
			return false;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.IsWelfare)
		{
			return player.ActivitySystem.HasWelfareRewardTaken(this.Info.ID);
		}
		return player.ActivitySystem.HasFundLevelRewardTaken(this.Info.ID);
	}

	public bool IsComplete()
	{
		if (this.Info == null)
		{
			return false;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.IsWelfare)
		{
			return player.ActivitySystem.BuyFundNum >= this.Info.BuyFundCount;
		}
		return player.ActivitySystem.HasBuyFund() && (ulong)player.Data.Level >= (ulong)((long)this.Info.FundLevel);
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
