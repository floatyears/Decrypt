using Proto;
using System;
using UnityEngine;

public sealed class ActivityShopDataEx : BaseData
{
	public ActivityShopData AShopData
	{
		get;
		private set;
	}

	public ActivityShopItem AShopItem
	{
		get;
		private set;
	}

	public ActivityShopDataEx(ActivityShopData shop, ActivityShopItem shopItem)
	{
		this.AShopData = shop;
		this.AShopItem = shopItem;
	}

	public void Buy()
	{
		if (this.AShopData == null || this.AShopItem == null)
		{
			return;
		}
		if (Tools.GetRemainAARewardTime(this.AShopData.Base.CloseTimeStamp) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			return;
		}
		if (this.AShopItem.MaxCount > 0 && this.AShopItem.MaxCount <= this.AShopItem.BuyCount)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityFlashSaleItemOver", 0f, 0f);
			return;
		}
		if (this.AShopItem.MaxTimes > 0 && this.AShopItem.MaxTimes <= this.AShopItem.BuyTimes)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityDartOver", 0f, 0f);
			return;
		}
		if (Tools.MoneyNotEnough((ECurrencyType)this.AShopItem.CurrencyType, this.AShopItem.Price, this.AShopItem.Price2))
		{
			return;
		}
		MC2S_BuyActivityShopItem mC2S_BuyActivityShopItem = new MC2S_BuyActivityShopItem();
		mC2S_BuyActivityShopItem.ActivityID = this.AShopData.Base.ID;
		mC2S_BuyActivityShopItem.ItemID = this.AShopItem.ID;
		mC2S_BuyActivityShopItem.Price = this.AShopItem.Price;
		Globals.Instance.CliSession.Send(753, mC2S_BuyActivityShopItem);
	}

	public string GetStepDesc()
	{
		if (this.AShopItem == null)
		{
			return string.Empty;
		}
		if (Tools.GetRemainAARewardTime(this.AShopData.Base.CloseTimeStamp) <= 0)
		{
			return Singleton<StringManager>.Instance.GetString("activityOverTip");
		}
		if (this.AShopItem.MaxCount > 0)
		{
			return Singleton<StringManager>.Instance.GetString("activityShop1", new object[]
			{
				this.AShopItem.MaxCount - this.AShopItem.BuyCount,
				this.AShopItem.MaxCount
			});
		}
		if (this.AShopItem.MaxTimes > 0)
		{
			return Singleton<StringManager>.Instance.GetString("activityShop2", new object[]
			{
				this.AShopItem.MaxTimes - this.AShopItem.BuyTimes,
				this.AShopItem.MaxTimes
			});
		}
		return string.Empty;
	}

	public Color GetStepColor()
	{
		if (this.AShopItem.MaxCount > 0)
		{
			return new Color32(254, 167, 0, 255);
		}
		return new Color32(254, 238, 189, 255);
	}

	public override ulong GetID()
	{
		if (this.AShopItem != null)
		{
			return (ulong)((long)this.AShopItem.ID);
		}
		return (ulong)((long)this.GetHashCode());
	}
}
