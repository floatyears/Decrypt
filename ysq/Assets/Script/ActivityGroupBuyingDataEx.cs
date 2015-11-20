using Proto;
using System;

public sealed class ActivityGroupBuyingDataEx : BaseData
{
	public ActivityGroupBuyingData mGUIGroupBuyingData
	{
		get;
		private set;
	}

	public ActivityGroupBuyingItem mGUIGroupBuyingItem
	{
		get;
		private set;
	}

	public ActivityGroupBuyingDataEx(ActivityGroupBuyingData data, ActivityGroupBuyingItem item)
	{
		this.mGUIGroupBuyingData = data;
		this.mGUIGroupBuyingItem = item;
	}

	public bool IsBuy()
	{
		return this.mGUIGroupBuyingItem.MyCount > 0 && this.mGUIGroupBuyingItem.Limit <= this.mGUIGroupBuyingItem.MyCount;
	}
}
