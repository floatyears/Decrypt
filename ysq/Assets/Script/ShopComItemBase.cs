using System;
using UnityEngine;

public abstract class ShopComItemBase : UICustomGridItem
{
	protected UISprite PriceIcon;

	protected UILabel PriceCount;

	protected UISprite PriceIcon1;

	protected UILabel PriceCount1;

	protected UILabel BuyCount;

	protected UILabel Hot;

	protected UISprite mLightBG;

	protected GameObject SoldOut;

	protected UIButton BtnBuy;

	protected ShopItemIcon ItemIcon;

	public virtual void Init()
	{
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.PriceIcon = base.transform.Find("PriceIcon0").GetComponent<UISprite>();
		this.PriceCount = this.PriceIcon.transform.Find("Price").GetComponent<UILabel>();
		this.PriceIcon1 = base.transform.Find("PriceIcon1").GetComponent<UISprite>();
		this.PriceCount1 = this.PriceIcon1.transform.Find("Price").GetComponent<UILabel>();
		this.PriceIcon1.gameObject.SetActive(false);
		this.BuyCount = base.transform.Find("BuyCount").GetComponent<UILabel>();
		this.Hot = base.transform.Find("Hot").GetComponent<UILabel>();
		this.mLightBG = base.transform.Find("LightBG").GetComponent<UISprite>();
		this.SoldOut = base.transform.Find("SoldOut").gameObject;
		this.BtnBuy = base.transform.Find("BtnBuy").GetComponent<UIButton>();
		UIEventListener expr_11E = UIEventListener.Get(this.BtnBuy.gameObject);
		expr_11E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11E.onClick, new UIEventListener.VoidDelegate(this.OnBuyShopItemClicked));
		this.ItemIcon = base.transform.Find("itemQuality").gameObject.AddComponent<ShopItemIcon>();
		this.ItemIcon.Init();
		UIEventListener expr_17B = UIEventListener.Get(this.ItemIcon.gameObject);
		expr_17B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_17B.onClick, new UIEventListener.VoidDelegate(this.OnShopItemClicked));
	}

	protected abstract void OnBuyShopItemClicked(GameObject go);

	protected abstract void OnShopItemClicked(GameObject go);
}
