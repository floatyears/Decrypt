using Proto;
using System;
using UnityEngine;

public class UIActivityShopItem : UICustomGridItem
{
	private ActivityShopDataEx Data;

	private Transform Reward;

	private UISprite CurrencyIcon;

	private UILabel CurrencyNum;

	private UILabel Step;

	private GameObject BtnBuy;

	private GameObject BtnOver;

	private GameObject CurrencyReward;

	private GameObject RTReward;

	public void Init()
	{
		this.Reward = base.transform.Find("Reward");
		this.CurrencyIcon = base.transform.Find("Currency/icon").GetComponent<UISprite>();
		this.CurrencyNum = base.transform.Find("Currency/num").GetComponent<UILabel>();
		this.Step = base.transform.Find("step").GetComponent<UILabel>();
		this.BtnBuy = base.transform.Find("GoBtn").gameObject;
		UIEventListener expr_8D = UIEventListener.Get(this.BtnBuy);
		expr_8D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8D.onClick, new UIEventListener.VoidDelegate(this.OnBtnBuyClicked));
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
		this.Data = (ActivityShopDataEx)_data;
		this.RefreshData();
	}

	private void RefreshBtnState()
	{
		if (this.Data == null || this.Data.AShopItem == null)
		{
			return;
		}
		if (Tools.GetCurrencyMoney((ECurrencyType)this.Data.AShopItem.CurrencyType, this.Data.AShopItem.Price2) >= this.Data.AShopItem.Price)
		{
			this.CurrencyNum.color = Color.white;
		}
		else
		{
			this.CurrencyNum.color = Color.red;
		}
		this.Step.text = this.Data.GetStepDesc();
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
			if (this.CurrencyReward != null)
			{
				UnityEngine.Object.Destroy(this.CurrencyReward);
				this.CurrencyReward = null;
			}
			this.CurrencyReward = GameUITools.CreateReward(3, this.Data.AShopItem.Price2, this.Data.AShopItem.Price, base.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			this.CurrencyReward.gameObject.AddComponent<UIDragScrollView>();
			this.CurrencyReward.transform.localPosition = this.CurrencyIcon.transform.localPosition;
			this.CurrencyReward.transform.localScale = this.CurrencyIcon.transform.localScale;
		}
		else
		{
			this.CurrencyIcon.gameObject.SetActive(true);
			this.CurrencyIcon.spriteName = Tools.GetCurrencyIcon((ECurrencyType)this.Data.AShopItem.CurrencyType);
			this.CurrencyNum.text = this.Data.AShopItem.Price.ToString();
		}
		if (this.RTReward != null)
		{
			UnityEngine.Object.Destroy(this.RTReward);
			this.RTReward = null;
		}
		this.RTReward = GameUITools.CreateReward(this.Data.AShopItem.Type, this.Data.AShopItem.Value1, this.Data.AShopItem.Value2, this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		this.RTReward.gameObject.AddComponent<UIDragScrollView>();
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
