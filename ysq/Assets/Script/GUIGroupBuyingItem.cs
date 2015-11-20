using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGroupBuyingItem : UICustomGridItem
{
	private ActivityGroupBuyingDataEx Data;

	private Transform mCurrencyIcon;

	private Transform mReward;

	private GameObject Slider;

	private UISprite mSF;

	private UILabel mTitle;

	private UILabel mPrice;

	private UILabel mCanUsed;

	private UILabel mTip;

	private UILabel mStep;

	private GameObject mBtnBuy;

	private GameObject mBtnOver;

	private GameObject mRTReward;

	private UILabel[] discount = new UILabel[5];

	private UILabel[] buyCount = new UILabel[5];

	public void Init()
	{
		this.mReward = base.transform.Find("Reward");
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mCurrencyIcon = base.transform.Find("currencyIcon");
		this.mPrice = this.mCurrencyIcon.transform.Find("price").GetComponent<UILabel>();
		this.mCanUsed = this.mCurrencyIcon.transform.Find("canUsed").GetComponent<UILabel>();
		this.mTip = base.transform.Find("tips").GetComponent<UILabel>();
		this.mStep = base.transform.Find("step").GetComponent<UILabel>();
		this.Slider = base.transform.Find("slider").gameObject;
		this.mSF = this.Slider.transform.Find("Fg").GetComponent<UISprite>();
		for (int i = 0; i < 5; i++)
		{
			this.discount[i] = this.Slider.transform.Find(string.Format("p{0}", i)).GetComponent<UILabel>();
			this.buyCount[i] = this.discount[i].transform.Find("num").GetComponent<UILabel>();
			this.discount[i].text = "0";
		}
		this.buyCount[0].text = string.Empty;
		this.mBtnBuy = base.transform.Find("GoBtn").gameObject;
		UIEventListener expr_1A5 = UIEventListener.Get(this.mBtnBuy);
		expr_1A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A5.onClick, new UIEventListener.VoidDelegate(this.OnBtnBuyClicked));
		this.mBtnOver = base.transform.Find("OverBtn").gameObject;
		ActivitySubSystem expr_1F0 = Globals.Instance.Player.ActivitySystem;
		expr_1F0.GBBuyItemEvent = (ActivitySubSystem.AGBCallBack)Delegate.Combine(expr_1F0.GBBuyItemEvent, new ActivitySubSystem.AGBCallBack(this.OnActivityGroupBuyingBuyEvent));
		ActivitySubSystem expr_220 = Globals.Instance.Player.ActivitySystem;
		expr_220.GetGroupBuyingDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_220.GetGroupBuyingDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetActivityGBEvent));
	}

	private void OnDestroy()
	{
		ActivitySubSystem expr_0F = Globals.Instance.Player.ActivitySystem;
		expr_0F.GBBuyItemEvent = (ActivitySubSystem.AGBCallBack)Delegate.Remove(expr_0F.GBBuyItemEvent, new ActivitySubSystem.AGBCallBack(this.OnActivityGroupBuyingBuyEvent));
		ActivitySubSystem expr_3F = Globals.Instance.Player.ActivitySystem;
		expr_3F.GetGroupBuyingDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_3F.GetGroupBuyingDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetActivityGBEvent));
	}

	public override void Refresh(object _data)
	{
		ActivityGroupBuyingDataEx activityGroupBuyingDataEx = (ActivityGroupBuyingDataEx)_data;
		if (activityGroupBuyingDataEx == this.Data && activityGroupBuyingDataEx.mGUIGroupBuyingData == this.Data.mGUIGroupBuyingData && activityGroupBuyingDataEx.mGUIGroupBuyingItem == this.Data.mGUIGroupBuyingItem)
		{
			this.RefreshBtnState();
			return;
		}
		this.Data = activityGroupBuyingDataEx;
		this.RefreshRewardData();
		this.RefreshSlider();
	}

	private void RefreshBtnState()
	{
		if (this.Data == null || this.Data.mGUIGroupBuyingItem == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.Data.mGUIGroupBuyingItem.Discounts.Count; i++)
		{
			if (this.Data.mGUIGroupBuyingItem.TotalCount >= this.Data.mGUIGroupBuyingItem.Discounts[i].BuyCount)
			{
				num = this.Data.mGUIGroupBuyingItem.Discounts[i].Discount;
			}
		}
		int diamond = this.Data.mGUIGroupBuyingItem.Diamond;
		this.mPrice.text = ((int)((float)num / 10000f * (float)diamond)).ToString();
		if (Tools.GetCurrencyMoney((ECurrencyType)this.Data.mGUIGroupBuyingItem.CostType, this.Data.mGUIGroupBuyingItem.Diamond) >= diamond)
		{
			this.mPrice.color = new Color32(252, 238, 189, 255);
		}
		else
		{
			this.mPrice.color = Color.red;
		}
		this.mStep.text = this.RefreshMyNum();
		this.mTip.text = Singleton<StringManager>.Instance.GetString("groupBuy_0");
		int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(221));
		if (itemCount < (int)((float)num / 10000f * (float)this.Data.mGUIGroupBuyingItem.Coupon))
		{
			this.mCanUsed.text = Singleton<StringManager>.Instance.GetString("groupBuy_4", new object[]
			{
				itemCount,
				(int)((float)num / 10000f * (float)this.Data.mGUIGroupBuyingItem.Coupon)
			});
			this.mCanUsed.color = Color.red;
		}
		else
		{
			this.mCanUsed.text = Singleton<StringManager>.Instance.GetString("groupBuy_4", new object[]
			{
				itemCount,
				(int)((float)num / 10000f * (float)this.Data.mGUIGroupBuyingItem.Coupon)
			});
			this.mCanUsed.color = Color.green;
		}
		int myCount = this.Data.mGUIGroupBuyingItem.MyCount;
		if (myCount > 0 && this.Data.mGUIGroupBuyingItem.Limit <= myCount)
		{
			this.mBtnBuy.SetActive(false);
			this.mBtnOver.SetActive(true);
		}
		else
		{
			this.mBtnBuy.SetActive(true);
			this.mBtnOver.SetActive(false);
		}
	}

	private string RefreshMyNum()
	{
		if (this.Data.mGUIGroupBuyingItem == null)
		{
			return string.Empty;
		}
		if (Tools.GetRemainAARewardTime(this.Data.mGUIGroupBuyingData.Base.CloseTimeStamp) <= 0)
		{
			return Singleton<StringManager>.Instance.GetString("activityOverTip");
		}
		if (this.Data.mGUIGroupBuyingItem.Limit > 0)
		{
			return Singleton<StringManager>.Instance.GetString("groupBuy_7", new object[]
			{
				this.Data.mGUIGroupBuyingItem.Limit - this.Data.mGUIGroupBuyingItem.MyCount,
				this.Data.mGUIGroupBuyingItem.Limit
			});
		}
		return string.Empty;
	}

	public void Buy()
	{
		if (this.Data.mGUIGroupBuyingData == null || this.Data.mGUIGroupBuyingItem == null)
		{
			return;
		}
		if (Tools.GetRemainAARewardTime(this.Data.mGUIGroupBuyingData.Base.CloseTimeStamp) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			return;
		}
		if (this.Data.mGUIGroupBuyingItem.Limit > 0 && this.Data.mGUIGroupBuyingItem.Limit <= this.Data.mGUIGroupBuyingItem.MyCount)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityDartOver", 0f, 0f);
			return;
		}
		int num = 0;
		for (int i = 0; i < this.Data.mGUIGroupBuyingItem.Discounts.Count; i++)
		{
			if (this.Data.mGUIGroupBuyingItem.TotalCount >= this.Data.mGUIGroupBuyingItem.Discounts[i].BuyCount)
			{
				num = this.Data.mGUIGroupBuyingItem.Discounts[i].Discount;
			}
		}
		int diamond = this.Data.mGUIGroupBuyingItem.Diamond;
		if (Tools.MoneyNotEnough((ECurrencyType)this.Data.mGUIGroupBuyingItem.CostType, (int)((float)num / 10000f * (float)diamond), 0))
		{
			return;
		}
		MC2S_ActivityGroupBuyingBuy mC2S_ActivityGroupBuyingBuy = new MC2S_ActivityGroupBuyingBuy();
		mC2S_ActivityGroupBuyingBuy.ID = this.Data.mGUIGroupBuyingItem.ID;
		Globals.Instance.CliSession.Send(775, mC2S_ActivityGroupBuyingBuy);
	}

	private void RefreshRewardData()
	{
		if (this.mRTReward != null)
		{
			UnityEngine.Object.Destroy(this.mRTReward);
			this.mRTReward = null;
		}
		this.mRTReward = GameUITools.CreateReward(this.Data.mGUIGroupBuyingItem.ItemType, this.Data.mGUIGroupBuyingItem.ItemID, this.Data.mGUIGroupBuyingItem.ItemCount, this.mReward.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		this.mRTReward.gameObject.AddComponent<UIDragScrollView>();
		this.mTitle.text = Tools.GetRewardTypeName(ERewardType.EReward_Item, this.Data.mGUIGroupBuyingItem.ItemID);
		this.RefreshBtnState();
	}

	private void RefreshSlider()
	{
		this.mSF.fillAmount = (float)this.Data.mGUIGroupBuyingItem.TotalCount / (float)this.Data.mGUIGroupBuyingItem.Discounts[this.Data.mGUIGroupBuyingItem.Discounts.Count - 1].BuyCount;
		if (this.mSF.fillAmount > 1f)
		{
			this.mSF.fillAmount = 1f;
		}
		for (int i = 0; i < this.Data.mGUIGroupBuyingItem.Discounts.Count; i++)
		{
			this.discount[i].text = Singleton<StringManager>.Instance.GetString("groupBuy_3", new object[]
			{
				(float)this.Data.mGUIGroupBuyingItem.Discounts[i].Discount / 1000f
			});
			this.buyCount[i].text = this.Data.mGUIGroupBuyingItem.Discounts[i].BuyCount.ToString();
			if (this.buyCount[i].text == "0")
			{
				this.buyCount[i].text = string.Empty;
			}
		}
	}

	private void OnBtnBuyClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		this.Buy();
	}

	public void OnActivityGroupBuyingBuyEvent(ActivityGroupBuyingItem data)
	{
		GUIRewardPanel.Show(new List<RewardData>
		{
			new RewardData
			{
				RewardType = data.ItemType,
				RewardValue1 = data.ItemID,
				RewardValue2 = data.ItemCount
			}
		}, null, false, true, null, false);
		this.RefreshMyNum();
		this.RefreshSlider();
		this.RefreshBtnState();
	}

	public void OnGetActivityGBEvent()
	{
		this.RefreshMyNum();
		this.RefreshSlider();
		this.RefreshBtnState();
	}
}
