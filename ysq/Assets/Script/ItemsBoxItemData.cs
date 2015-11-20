using Proto;
using System;

public class ItemsBoxItemData : BaseData
{
	public RewardData data;

	public ItemsBoxItemData(RewardData data)
	{
		this.data = data;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.data.RewardType);
	}
}
