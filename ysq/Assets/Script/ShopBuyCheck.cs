using Proto;
using System;
using UnityEngine;

public class ShopBuyCheck : MonoBehaviour
{
	private RTShopGridData rtShopData;

	private ShopGridData shopData;

	protected ShopItemIcon ItemIcon;

	private UISprite PriceIcon;

	private UILabel PriceCount;

	private UISprite PriceIcon1;

	private UILabel PriceCount1;

	private UILabel Hot;

	private UIButton ok;

	private UIButton cancel;

	public void Init()
	{
		this.ItemIcon = base.transform.Find("itemQuality").gameObject.AddComponent<ShopItemIcon>();
		this.ItemIcon.Init();
		this.PriceIcon = base.transform.Find("PriceIcon").GetComponent<UISprite>();
		this.PriceCount = this.PriceIcon.transform.Find("Price").GetComponent<UILabel>();
		this.PriceIcon1 = base.transform.Find("PriceIcon1").GetComponent<UISprite>();
		this.PriceCount1 = this.PriceIcon1.transform.Find("Price").GetComponent<UILabel>();
		this.Hot = base.transform.Find("Hot").GetComponent<UILabel>();
		this.ok = base.transform.Find("BtnBuy").GetComponent<UIButton>();
		UIEventListener expr_E7 = UIEventListener.Get(this.ok.gameObject);
		expr_E7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E7.onClick, new UIEventListener.VoidDelegate(this.OnBuyClicked));
		this.cancel = base.transform.Find("BtnCancel").GetComponent<UIButton>();
		UIEventListener expr_133 = UIEventListener.Get(this.cancel.gameObject);
		expr_133.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_133.onClick, new UIEventListener.VoidDelegate(this.OnCancelClicked));
	}

	public void Show(RTShopGridData data)
	{
		this.rtShopData = data;
		this.shopData = null;
		if (this.rtShopData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.ItemIcon.Refresh(this.rtShopData);
		this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.rtShopData.shopData.Type);
		this.PriceCount.text = this.rtShopData.GetPrice().ToString();
		this.PriceIcon1.gameObject.SetActive(false);
		this.Hot.gameObject.SetActive(this.rtShopData.AVData != null);
		base.gameObject.SetActive(true);
	}

	public void Show(ShopGridData data)
	{
		this.rtShopData = null;
		this.shopData = data;
		if (this.shopData == null || this.shopData.shopInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.ItemIcon.Refresh(this.shopData);
		this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.shopData.shopInfo.CurrencyType);
		this.PriceCount.text = this.shopData.shopInfo.Price.ToString();
		this.Hot.gameObject.SetActive(false);
		if (this.shopData.shopInfo.Price2 > 0)
		{
			this.PriceIcon1.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.shopData.shopInfo.CurrencyType2);
			this.PriceCount1.text = this.shopData.shopInfo.Price2.ToString();
			this.PriceIcon1.gameObject.SetActive(true);
		}
		else
		{
			this.PriceIcon1.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
	}

	private void OnBuyClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_026");
		if (this.rtShopData != null)
		{
			GUIShortcutBuyItem.RequestShopBuyRTItem(this.rtShopData, this.rtShopData.ShopType);
		}
		else if (this.shopData != null)
		{
			GUIShortcutBuyItem.RequestShopBuyItem(this.shopData.shopInfo, this.shopData.ShopType, 1);
		}
		this.rtShopData = null;
		this.shopData = null;
		base.gameObject.SetActive(false);
	}

	private void OnCancelClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.rtShopData = null;
		this.shopData = null;
		base.gameObject.SetActive(false);
	}
}
