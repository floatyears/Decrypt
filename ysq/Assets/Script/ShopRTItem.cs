using Proto;
using System;
using UnityEngine;

public sealed class ShopRTItem : ShopComItemBase
{
	public RTShopGridData gridData
	{
		get;
		private set;
	}

	public override void Refresh(object data)
	{
		this.gridData = (RTShopGridData)data;
		this.RefreshShopInfo();
	}

	private void RefreshShopInfo()
	{
		if (this.gridData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.ItemIcon.Refresh(this.gridData);
		this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.gridData.shopData.Type);
		this.PriceIcon.MakePixelPerfect();
		uint price = (uint)this.gridData.GetPrice();
		this.PriceCount.text = price.ToString();
		if ((long)Tools.GetCurrencyMoney((ECurrencyType)this.gridData.shopData.Type, 0) < (long)((ulong)price))
		{
			this.PriceCount.color = Color.red;
		}
		else
		{
			this.PriceCount.color = Color.white;
		}
		this.RefreshHot();
		this.SoldOut.gameObject.SetActive(false);
		this.BuyCount.gameObject.SetActive(false);
		if (this.gridData.shopData.Count == 0u)
		{
			this.BtnBuy.isEnabled = true;
		}
		else if (this.gridData.shopData.BuyCount == 0u)
		{
			this.BtnBuy.isEnabled = true;
		}
		else
		{
			this.BtnBuy.isEnabled = false;
			this.SoldOut.gameObject.SetActive(true);
		}
		if (this.mLightBG != null)
		{
			if ((this.gridData.shopData.Flag & 1) != 0)
			{
				this.mLightBG.gameObject.SetActive(true);
			}
			else
			{
				this.mLightBG.gameObject.SetActive(false);
			}
		}
	}

	private void RefreshHot()
	{
		if (this.gridData.AVData != null)
		{
			this.Hot.text = Tools.FormatOffPrice(this.gridData.AVData.Value1);
			this.Hot.gameObject.SetActive(true);
			return;
		}
		if (this.gridData.IsHot())
		{
			this.Hot.gameObject.SetActive(true);
			this.Hot.text = Singleton<StringManager>.Instance.GetString("recommendText2");
		}
		else
		{
			this.Hot.gameObject.SetActive(false);
		}
	}

	protected override void OnBuyShopItemClicked(GameObject go)
	{
		if (this.gridData == null || this.gridData.shopData == null)
		{
			return;
		}
		this.gridData.OnBuyShopItem();
	}

	protected override void OnShopItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.gridData == null || this.gridData.shopData == null)
		{
			return;
		}
		this.gridData.OnShowShopItem();
	}
}
