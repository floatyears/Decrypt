using Att;
using Proto;
using System;
using UnityEngine;

public class UIActivityFlashShopItem : UICustomGridItem
{
	private ActivityShopDataEx Data;

	private Transform CurrencyIcon;

	private Transform CurrencyItem;

	private Transform Reward;

	private UILabel CurrencyNum;

	private UILabel Title;

	private UILabel Hot;

	private UILabel Price;

	private UISprite PriceIcon;

	private UILabel OffPrice;

	private UISprite OffPriceIcon;

	private GameObject Line;

	private UILabel Tip;

	private UILabel Step;

	private GameObject BtnBuy;

	private GameObject BtnOver;

	private GameObject CurrencyReward;

	private GameObject RTReward;

	public void Init()
	{
		this.Reward = base.transform.Find("Reward");
		this.CurrencyItem = base.transform.Find("CurrencyItem");
		this.Title = base.transform.Find("Title").GetComponent<UILabel>();
		this.CurrencyIcon = base.transform.Find("CurrencyIcon");
		this.Hot = this.CurrencyIcon.transform.Find("Hot").GetComponent<UILabel>();
		this.Price = this.CurrencyIcon.transform.Find("Price").GetComponent<UILabel>();
		this.PriceIcon = this.Price.transform.Find("icon").GetComponent<UISprite>();
		this.OffPrice = this.CurrencyIcon.transform.Find("OffPrice").GetComponent<UILabel>();
		this.OffPriceIcon = this.OffPrice.transform.Find("icon").GetComponent<UISprite>();
		this.Line = this.Price.transform.Find("line").gameObject;
		this.Tip = this.Price.transform.Find("tip").GetComponent<UILabel>();
		this.Step = base.transform.Find("step").GetComponent<UILabel>();
		this.BtnBuy = base.transform.Find("GoBtn").gameObject;
		UIEventListener expr_17E = UIEventListener.Get(this.BtnBuy);
		expr_17E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_17E.onClick, new UIEventListener.VoidDelegate(this.OnBtnBuyClicked));
		this.BtnOver = base.transform.Find("OverBtn").gameObject;
	}

	public override void Refresh(object _data)
	{
		ActivityShopDataEx activityShopDataEx = (ActivityShopDataEx)_data;
		if (activityShopDataEx == this.Data && activityShopDataEx.AShopData == this.Data.AShopData && activityShopDataEx.AShopItem == this.Data.AShopItem)
		{
			this.RefreshBtnState();
			return;
		}
		this.Data = activityShopDataEx;
		this.RefreshData();
	}

	private void RefreshBtnState()
	{
		if (this.Data == null || this.Data.AShopItem == null)
		{
			return;
		}
		if (this.Data.AShopItem.CurrencyType == 8)
		{
			int price = this.Data.AShopItem.Price;
			if (Tools.GetCurrencyMoney((ECurrencyType)this.Data.AShopItem.CurrencyType, this.Data.AShopItem.Price2) >= price)
			{
				this.CurrencyNum.color = Color.white;
			}
			else
			{
				this.CurrencyNum.color = Color.red;
			}
		}
		else
		{
			int price2 = this.Data.AShopItem.Price;
			int originalPrice = this.Data.AShopItem.OriginalPrice;
			if (originalPrice > 0 && price2 < originalPrice)
			{
				if (Tools.GetCurrencyMoney((ECurrencyType)this.Data.AShopItem.CurrencyType, this.Data.AShopItem.Price2) >= price2)
				{
					this.OffPrice.color = Color.white;
				}
				else
				{
					this.OffPrice.color = Color.red;
				}
			}
			else if (Tools.GetCurrencyMoney((ECurrencyType)this.Data.AShopItem.CurrencyType, this.Data.AShopItem.Price2) >= price2)
			{
				this.Price.color = new Color32(252, 238, 189, 255);
			}
			else
			{
				this.Price.color = Color.red;
			}
		}
		this.Step.text = this.Data.GetStepDesc();
		this.Step.color = this.Data.GetStepColor();
		if (this.Data.AShopItem.MaxCount > 0 && this.Data.AShopItem.MaxCount <= this.Data.AShopItem.BuyCount)
		{
			this.BtnBuy.SetActive(false);
			this.BtnOver.SetActive(true);
		}
		if (this.Data.AShopItem.MaxTimes > 0 && this.Data.AShopItem.MaxTimes <= this.Data.AShopItem.BuyTimes)
		{
			this.BtnBuy.SetActive(false);
			this.BtnOver.SetActive(true);
		}
		else
		{
			this.BtnBuy.SetActive(true);
			this.BtnOver.SetActive(false);
		}
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.AShopItem == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.Data.AShopItem.CurrencyType == 8)
		{
			this.CurrencyIcon.gameObject.SetActive(false);
			this.CurrencyItem.gameObject.SetActive(true);
			if (this.CurrencyReward != null)
			{
				UnityEngine.Object.Destroy(this.CurrencyReward);
				this.CurrencyReward = null;
			}
			this.CurrencyReward = GameUITools.CreateMinReward(3, this.Data.AShopItem.Price2, this.Data.AShopItem.Price, this.CurrencyItem);
			this.CurrencyReward.gameObject.AddComponent<UIDragScrollView>();
			this.CurrencyReward.transform.localPosition = this.CurrencyIcon.transform.localPosition;
			this.CurrencyReward.transform.localScale = this.CurrencyIcon.transform.localScale;
			this.CurrencyNum = this.CurrencyReward.transform.Find("num").GetComponent<UILabel>();
		}
		else
		{
			this.CurrencyIcon.gameObject.SetActive(true);
			this.CurrencyItem.gameObject.SetActive(false);
			int price = this.Data.AShopItem.Price;
			int originalPrice = this.Data.AShopItem.OriginalPrice;
			if (originalPrice > 0 && price < originalPrice)
			{
				int num = Mathf.CeilToInt((float)price / (float)originalPrice * 100f);
				if (0 < num && num < 100)
				{
					this.Hot.gameObject.SetActive(true);
					this.Hot.text = Tools.FormatOffPrice(num);
				}
				else
				{
					this.Hot.gameObject.SetActive(false);
				}
				this.Price.text = originalPrice.ToString();
				this.OffPrice.text = price.ToString();
				this.Line.SetActive(true);
				this.OffPrice.gameObject.SetActive(true);
				this.Tip.text = Singleton<StringManager>.Instance.GetString("activityShop5");
			}
			else
			{
				this.Hot.gameObject.SetActive(false);
				this.Price.text = price.ToString();
				this.Line.SetActive(false);
				this.OffPrice.gameObject.SetActive(false);
				this.Tip.text = Singleton<StringManager>.Instance.GetString("activityShop4");
			}
			this.PriceIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.Data.AShopItem.CurrencyType);
			this.OffPriceIcon.spriteName = this.PriceIcon.spriteName;
		}
		if (this.RTReward != null)
		{
			UnityEngine.Object.Destroy(this.RTReward);
			this.RTReward = null;
		}
		this.RTReward = GameUITools.CreateReward(this.Data.AShopItem.Type, this.Data.AShopItem.Value1, this.Data.AShopItem.Value2, this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		this.RTReward.gameObject.AddComponent<UIDragScrollView>();
		this.Title.text = Tools.GetRewardTypeName((ERewardType)this.Data.AShopItem.Type, this.Data.AShopItem.Value1);
		this.RefreshBtnState();
		base.gameObject.SetActive(true);
	}

	private void OnBtnBuyClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		this.Data.Buy();
	}
}
