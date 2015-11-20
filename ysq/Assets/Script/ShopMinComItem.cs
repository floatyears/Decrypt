using Proto;
using System;
using UnityEngine;

public class ShopMinComItem : ShopComItemBase
{
	private ShopGridData gridData;

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
		this.ItemIcon.Refresh(this.gridData);
		if (this.gridData.shopInfo.Price2 == 0)
		{
			this.PriceIcon1.gameObject.SetActive(false);
		}
		else
		{
			this.PriceIcon1.gameObject.SetActive(true);
			this.PriceIcon1.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.gridData.shopInfo.CurrencyType2);
			this.PriceIcon1.MakePixelPerfect();
			this.PriceCount1.text = this.gridData.shopInfo.Price2.ToString();
			if (Tools.GetCurrencyMoney((ECurrencyType)this.gridData.shopInfo.CurrencyType2, 0) < this.gridData.shopInfo.Price2)
			{
				this.PriceCount1.color = Color.red;
			}
			else
			{
				this.PriceCount1.color = Color.white;
			}
		}
		int buyCount = player.GetBuyCount(this.gridData.shopInfo);
		int num = (this.gridData.shopInfo.IsFashion != 0) ? this.gridData.shopInfo.Price : Tools.GetItemBuyConst(this.gridData.itemInfo, buyCount + 1, this.gridData.shopInfo);
		this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.gridData.shopInfo.CurrencyType);
		this.PriceIcon.MakePixelPerfect();
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
		this.SoldOut.gameObject.SetActive(false);
		if (this.gridData.shopInfo.Type == 1 && (player.GuildSystem.Guild == null || player.GuildSystem.Guild.Level < this.gridData.shopInfo.Value))
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT4"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
			this.BuyCount.gameObject.SetActive(true);
		}
		else if (this.gridData.shopInfo.Type == 2 && player.Data.TrialMaxWave < this.gridData.shopInfo.Value)
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT41"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
			this.BuyCount.gameObject.SetActive(true);
		}
		else if (this.gridData.shopInfo.Type == 3 && (this.gridData.shopInfo.Value < player.Data.ArenaHighestRank || player.Data.ArenaHighestRank == 0))
		{
			this.BuyCount.text = string.Format(Singleton<StringManager>.Instance.GetString("ShopT42"), this.gridData.shopInfo.Value);
			this.BuyCount.color = Color.red;
			this.BtnBuy.isEnabled = false;
			this.BuyCount.gameObject.SetActive(true);
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

	protected override void OnBuyShopItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.gridData == null || this.gridData.shopInfo == null)
		{
			return;
		}
		this.gridData.OnBuyShopItem();
	}

	protected override void OnShopItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.gridData == null || this.gridData.shopInfo == null)
		{
			return;
		}
		this.gridData.OnShowShopItem();
	}
}
