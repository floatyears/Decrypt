using Proto;
using System;
using UnityEngine;

public class ShopCommonItem : UICustomGridItem
{
	private UISprite PriceIcon;

	private UILabel PriceCount;

	private UILabel BuyCount;

	private UILabel Hot;

	private GameObject SoldOut;

	private UIButton BtnBuy;

	private ShopItemIcon ItemIcon;

	private ShopGridData gridData;

	public void Init()
	{
		this.CreateObjects();
	}

	public override void Refresh(object data)
	{
		this.gridData = (ShopGridData)data;
		this.RefreshShopInfo();
	}

	private void RefreshShopInfo()
	{
		if (this.gridData.shopInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.gridData.shopInfo.IsFashion != 0 && this.gridData.fashionInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.gridData.shopInfo.IsFashion == 0 && this.gridData.itemInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		int buyCount = player.GetBuyCount(this.gridData.shopInfo);
		int num;
		if (this.gridData.shopInfo.IsFashion != 0)
		{
			num = this.gridData.shopInfo.Price;
		}
		else
		{
			num = Tools.GetItemBuyConst(this.gridData.itemInfo, buyCount + 1, this.gridData.shopInfo);
		}
		this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.gridData.shopInfo.CurrencyType);
		this.PriceCount.text = num.ToString();
		if (Tools.GetCurrencyMoney((ECurrencyType)this.gridData.shopInfo.CurrencyType, 0) < num)
		{
			this.PriceCount.color = Color.red;
		}
		else
		{
			this.PriceCount.color = Color.white;
		}
		this.Hot.gameObject.SetActive(false);
		this.ItemIcon.Refresh(this.gridData);
		this.SoldOut.gameObject.SetActive(false);
		if (this.gridData.shopInfo.Type == 1 && (player.GuildSystem.Guild == null || player.GuildSystem.Guild.Level < this.gridData.shopInfo.Value))
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT4"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
		}
		else if (this.gridData.shopInfo.Type == 2 && player.Data.TrialMaxWave < this.gridData.shopInfo.Value)
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT41"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
		}
		else if (this.gridData.shopInfo.Type == 3 && (this.gridData.shopInfo.Value < player.Data.ArenaHighestRank || player.Data.ArenaHighestRank == 0))
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT42"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
		}
		else if (this.gridData.shopInfo.Type == 4 && (ulong)player.Data.VipLevel < (ulong)((long)this.gridData.shopInfo.Value))
		{
			this.BuyCount.text = Singleton<StringManager>.Instance.GetString("ShopT43", new object[]
			{
				this.gridData.shopInfo.Value
			});
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
		}
		else if (this.gridData.shopInfo.IsFashion != 0)
		{
			this.BuyCount.gameObject.SetActive(false);
			bool flag = player.ItemSystem.HasFashion(this.gridData.fashionInfo.ID);
			this.SoldOut.gameObject.SetActive(flag);
			this.BtnBuy.isEnabled = !flag;
		}
		else if (this.gridData.shopInfo.Times == 0)
		{
			this.BuyCount.gameObject.SetActive(false);
			this.BtnBuy.isEnabled = true;
		}
		else
		{
			this.BuyCount.gameObject.SetActive(true);
			int num2 = Tools.GetShopBuyTimes(this.gridData.shopInfo) - buyCount;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 0)
			{
				this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT1"), num2);
				this.BuyCount.color = new Color32(244, 199, 159, 255);
				this.BtnBuy.isEnabled = true;
			}
			else
			{
				this.BuyCount.text = Singleton<StringManager>.Instance.GetString("ShopT3");
				this.BuyCount.color = Color.red;
				this.BtnBuy.isEnabled = false;
				this.SoldOut.gameObject.SetActive(true);
			}
		}
	}

	private void CreateObjects()
	{
		this.PriceIcon = base.transform.Find("PriceIcon").GetComponent<UISprite>();
		this.PriceCount = base.transform.Find("Price").GetComponent<UILabel>();
		this.BuyCount = base.transform.Find("BuyCount").GetComponent<UILabel>();
		this.Hot = base.transform.Find("Hot").GetComponent<UILabel>();
		this.SoldOut = base.transform.Find("SoldOut").gameObject;
		this.BtnBuy = base.transform.Find("BtnBuy").GetComponent<UIButton>();
		UIEventListener expr_B2 = UIEventListener.Get(this.BtnBuy.gameObject);
		expr_B2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B2.onClick, new UIEventListener.VoidDelegate(this.OnBuyShopItemClicked));
		this.ItemIcon = base.transform.Find("itemQuality").gameObject.AddComponent<ShopItemIcon>();
		this.ItemIcon.Init();
		UIEventListener expr_10E = UIEventListener.Get(this.ItemIcon.gameObject);
		expr_10E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10E.onClick, new UIEventListener.VoidDelegate(this.OnShopItemClicked));
	}

	private void OnBuyShopItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.gridData == null || this.gridData.shopInfo == null)
		{
			return;
		}
		this.gridData.OnBuyShopItem();
	}

	private void OnShopItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.gridData == null || this.gridData.shopInfo == null)
		{
			return;
		}
		this.gridData.OnShowShopItem();
	}
}
