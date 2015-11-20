using Proto;
using System;

public sealed class APItemDataEx : BaseData
{
	public ActivityPayShopData APSD
	{
		get;
		private set;
	}

	public APItemData APData
	{
		get;
		private set;
	}

	public APItemDataEx(ActivityPayShopData aa, APItemData data)
	{
		this.APSD = aa;
		this.APData = data;
	}

	public bool IsComplete()
	{
		return this.APSD.PayDay >= this.APData.Value;
	}

	public int BuyCount()
	{
		return this.APData.MaxCount - this.APData.BuyCount;
	}

	public string GetTitle()
	{
		return Singleton<StringManager>.Instance.GetString("activityShop3", new object[]
		{
			this.APData.Value
		});
	}

	public void Buy()
	{
		if (this.APSD == null || this.APData == null)
		{
			return;
		}
		if (Tools.GetRemainAARewardTime(this.APSD.Base.CloseTimeStamp) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			return;
		}
		if (this.BuyCount() <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityDartOver", 0f, 0f);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.APData.OffPrice, 0))
		{
			return;
		}
		MC2S_BuyActivityPayShopItem mC2S_BuyActivityPayShopItem = new MC2S_BuyActivityPayShopItem();
		mC2S_BuyActivityPayShopItem.ActivityID = this.APSD.Base.ID;
		mC2S_BuyActivityPayShopItem.ItemID = this.APData.ID;
		mC2S_BuyActivityPayShopItem.Price = this.APData.OffPrice;
		Globals.Instance.CliSession.Send(759, mC2S_BuyActivityPayShopItem);
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
