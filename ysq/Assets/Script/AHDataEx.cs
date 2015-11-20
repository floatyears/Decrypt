using Proto;
using System;
using System.Collections.Generic;

public sealed class AHDataEx : BaseData
{
	public List<RewardData> mItemID
	{
		get;
		private set;
	}

	public int mNum
	{
		get;
		private set;
	}

	public bool mNotOpen
	{
		get;
		private set;
	}

	public bool mUnlucky
	{
		get;
		private set;
	}

	public AHDataEx(ActivityHalloweenData ahd, int id, int num, bool notOpen, bool unlucky, bool lucky)
	{
		List<RewardData> mItemID = null;
		if (lucky)
		{
			for (int i = 0; i < ahd.Ext.Data.Count; i++)
			{
				if (ahd.Ext.Data[i].ID == id)
				{
					mItemID = ahd.Ext.Data[i].Rewards;
					break;
				}
			}
		}
		this.mItemID = mItemID;
		this.mNum = num;
		this.mNotOpen = notOpen;
		this.mUnlucky = unlucky;
	}
}
