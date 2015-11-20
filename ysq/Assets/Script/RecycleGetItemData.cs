using Proto;
using System;

public class RecycleGetItemData : BaseData
{
	public RewardData data;

	public RecycleGetItemData(RewardData data)
	{
		this.data = data;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.data.RewardType);
	}
}
