using Proto;
using System;

public class GUIGuildLootItemData : BaseData
{
	public RewardData mRewardData
	{
		get;
		private set;
	}

	public GUIGuildLootItemData(RewardData rd)
	{
		this.mRewardData = rd;
	}

	public override ulong GetID()
	{
		return (this.mRewardData == null) ? 0uL : CRC32.GetCrc(string.Format("{0}{1}{2}", this.mRewardData.RewardType, this.mRewardData.RewardValue1, this.mRewardData.RewardValue2));
	}
}
