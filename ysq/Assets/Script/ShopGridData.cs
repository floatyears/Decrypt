using Att;
using Proto;
using System;

public class ShopGridData : BaseData
{
	public delegate void ShopItemCallback(ShopGridData data);

	public ShopGridData.ShopItemCallback BuyShopItemEvent;

	public ShopGridData.ShopItemCallback ShowShopItemEvent;

	public EShopType ShopType
	{
		get;
		private set;
	}

	public ShopInfo shopInfo
	{
		get;
		private set;
	}

	public ItemInfo itemInfo
	{
		get;
		private set;
	}

	public FashionInfo fashionInfo
	{
		get;
		private set;
	}

	public ShopGridData(EShopType shopType, ShopInfo info, ShopGridData.ShopItemCallback buyCallback, ShopGridData.ShopItemCallback showCallback)
	{
		this.ShopType = shopType;
		this.shopInfo = info;
		if (info.IsFashion != 0)
		{
			this.fashionInfo = Globals.Instance.AttDB.FashionDict.GetInfo(this.shopInfo.InfoID);
		}
		else
		{
			this.itemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.shopInfo.InfoID);
		}
		this.BuyShopItemEvent = buyCallback;
		this.ShowShopItemEvent = showCallback;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}

	public void OnBuyShopItem()
	{
		if (this.BuyShopItemEvent != null)
		{
			this.BuyShopItemEvent(this);
		}
	}

	public void OnShowShopItem()
	{
		if (this.ShowShopItemEvent != null)
		{
			this.ShowShopItemEvent(this);
		}
	}
}
