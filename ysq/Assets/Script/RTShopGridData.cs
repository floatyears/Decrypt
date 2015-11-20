using Att;
using Proto;
using System;

public class RTShopGridData : BaseData
{
	public delegate void ShopItemCallback(RTShopGridData data);

	public RTShopGridData.ShopItemCallback BuyShopItemEvent;

	public RTShopGridData.ShopItemCallback ShowShopItemEvent;

	public EShopType ShopType
	{
		get;
		private set;
	}

	public ShopItemData shopData
	{
		get;
		private set;
	}

	public ItemInfo itemInfo
	{
		get;
		private set;
	}

	public PetInfo petInfo
	{
		get;
		private set;
	}

	public LopetInfo lopetInfo
	{
		get;
		private set;
	}

	public ActivityValueData AVData
	{
		get;
		private set;
	}

	public RTShopGridData(EShopType shopType, ShopItemData data, RTShopGridData.ShopItemCallback buyCallback, RTShopGridData.ShopItemCallback showCallback)
	{
		this.ShopType = shopType;
		this.shopData = data;
		this.AVData = Globals.Instance.Player.ActivitySystem.GetValueMod(7);
		if (this.shopData.InfoType == 1)
		{
			this.itemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.shopData.InfoID);
			if (this.itemInfo.Type == 3 && this.itemInfo.SubType == 0)
			{
				this.petInfo = Globals.Instance.AttDB.PetDict.GetInfo(this.itemInfo.Value2);
			}
			else if (this.itemInfo.Type == 3 && this.itemInfo.SubType == 3)
			{
				this.lopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(this.itemInfo.Value2);
			}
		}
		else if (this.shopData.InfoType == 2)
		{
			this.petInfo = Globals.Instance.AttDB.PetDict.GetInfo(this.shopData.InfoID);
		}
		else if (this.shopData.InfoType == 3)
		{
			this.lopetInfo = Globals.Instance.AttDB.LopetDict.GetInfo(this.shopData.InfoID);
		}
		this.BuyShopItemEvent = buyCallback;
		this.ShowShopItemEvent = showCallback;
	}

	public int GetPrice()
	{
		if (this.AVData == null)
		{
			return (int)this.shopData.Price;
		}
		return (int)(this.shopData.Price * (uint)this.AVData.Value1 / 100u);
	}

	public bool IsHot()
	{
		if (this.shopData.InfoType == 1)
		{
			if (this.itemInfo == null)
			{
				return false;
			}
			if (this.ShopType == EShopType.EShop_Awaken)
			{
				return Globals.Instance.Player.PetSystem.IsAwakeNeedItem(this.itemInfo.ID);
			}
			return this.itemInfo.Quality >= 3;
		}
		else
		{
			if (this.shopData.InfoType == 2)
			{
				return this.petInfo != null && this.petInfo.Quality >= 3;
			}
			return this.shopData.InfoType == 3 && this.lopetInfo != null && this.lopetInfo.Quality >= 3;
		}
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
