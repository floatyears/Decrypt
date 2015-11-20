using Att;
using Proto;
using System;
using UnityEngine;

public sealed class ASPItemDataEx : BaseData
{
	public ActivitySpecifyPayData ASPData
	{
		get;
		private set;
	}

	public ActivitySpecifyPayItem ASPItem
	{
		get;
		private set;
	}

	public ASPItemDataEx(ActivitySpecifyPayData asp, ActivitySpecifyPayItem item)
	{
		this.ASPData = asp;
		this.ASPItem = item;
	}

	public bool IsComplete()
	{
		return this.ASPItem.MaxCount > 0 && this.ASPItem.RewardCount >= this.ASPItem.MaxCount;
	}

	public bool IsNotTakeReward()
	{
		return this.ASPItem.PayCount > this.ASPItem.RewardCount;
	}

	public string GetTitle()
	{
		PayInfo info = Globals.Instance.AttDB.PayDict.GetInfo(this.ASPItem.ProductID);
		int num = 500;
		if (info != null)
		{
			num = Mathf.CeilToInt(info.Price);
		}
		return Singleton<StringManager>.Instance.GetString("ASPT_0", new object[]
		{
			num
		});
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
