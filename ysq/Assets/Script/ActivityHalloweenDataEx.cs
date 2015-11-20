using Proto;
using System;
using System.Collections.Generic;

public sealed class ActivityHalloweenDataEx : BaseData
{
	public int mProductID
	{
		get;
		private set;
	}

	public List<RewardData> mItemID
	{
		get;
		private set;
	}

	public int mLuckyNum
	{
		get;
		private set;
	}

	public string mPlayerName
	{
		get;
		private set;
	}

	public ActivityHalloweenDataEx(ActivityHalloweenData ahd, int id, int luckyNum, string playerName)
	{
		this.mProductID = id;
		this.mItemID = null;
		if (luckyNum != 0)
		{
			for (int i = 0; i < ahd.Ext.Data.Count; i++)
			{
				if (ahd.Ext.Data[i].ID == id)
				{
					this.mItemID = ahd.Ext.Data[i].Rewards;
					break;
				}
			}
		}
		this.mLuckyNum = luckyNum;
		this.mPlayerName = playerName;
	}

	public ActivityHalloweenDataEx(int id)
	{
		this.mProductID = id;
	}
}
