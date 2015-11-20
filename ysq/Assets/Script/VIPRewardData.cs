using Att;
using System;

public class VIPRewardData : BaseData
{
	public VipLevelInfo VipInfo
	{
		get;
		private set;
	}

	public VIPRewardData(VipLevelInfo info)
	{
		this.VipInfo = info;
	}

	public bool IsPayVipRewardTaken()
	{
		return Globals.Instance.Player.IsPayVipRewardTaken(this.VipInfo);
	}

	public int GetSortIndex()
	{
		return this.GetVipLevel();
	}

	public int GetVipLevel()
	{
		return Tools.GetVipLevel(this.VipInfo);
	}

	public string GetPayRewardTitle()
	{
		return Tools.GetVIPPayRewardTitle(this.VipInfo);
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
