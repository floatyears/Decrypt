using Proto;
using System;
using UnityEngine;

public class FundRewardItem : UICustomGridItem
{
	private FundRewardData Data;

	private UISprite bg;

	private UILabel Title;

	private Transform Reward;

	private GameObject RewardItem;

	private GameObject ReceiveBtn;

	private GameObject NoTake;

	private GameObject finished;

	public void Init()
	{
		this.bg = base.gameObject.GetComponent<UISprite>();
		this.Title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.Reward = base.transform.Find("Reward");
		this.ReceiveBtn = base.transform.FindChild("ReceiveBtn").gameObject;
		this.NoTake = base.transform.FindChild("NoTake").gameObject;
		this.finished = base.transform.FindChild("finished").gameObject;
		UIEventListener expr_A3 = UIEventListener.Get(this.ReceiveBtn.gameObject);
		expr_A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A3.onClick, new UIEventListener.VoidDelegate(this.OnReceiveBtnClicked));
	}

	private void OnReceiveBtnClicked(GameObject go)
	{
		if (this.Data == null || this.Data.Info == null)
		{
			return;
		}
		if (!this.Data.IsComplete() || this.Data.IsTakeReward())
		{
			return;
		}
		if (this.Data.IsWelfare)
		{
			MC2S_TakeWelfare mC2S_TakeWelfare = new MC2S_TakeWelfare();
			mC2S_TakeWelfare.ID = this.Data.Info.ID;
			Globals.Instance.CliSession.Send(739, mC2S_TakeWelfare);
		}
		else
		{
			MC2S_TakeFundLevelReward mC2S_TakeFundLevelReward = new MC2S_TakeFundLevelReward();
			mC2S_TakeFundLevelReward.ID = this.Data.Info.ID;
			Globals.Instance.CliSession.Send(737, mC2S_TakeFundLevelReward);
		}
	}

	public override void Refresh(object _data)
	{
		if (_data == this.Data)
		{
			this.RefreshBtnState();
			return;
		}
		this.Data = (FundRewardData)_data;
		this.RefreshData();
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.Info == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!this.Data.IsWelfare)
		{
			this.Title.text = Singleton<StringManager>.Instance.GetString("activityFund1", new object[]
			{
				this.Data.Info.FundLevel
			});
			if (this.RewardItem != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem);
				this.RewardItem = null;
			}
			this.RewardItem = GameUITools.CreateMinReward(2, this.Data.Info.FundDiamond, this.Data.Info.FundDiamond, this.Reward);
		}
		else
		{
			this.Title.text = Singleton<StringManager>.Instance.GetString("activityFund2", new object[]
			{
				this.Data.Info.BuyFundCount
			});
			if (this.RewardItem != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem);
				this.RewardItem = null;
			}
			this.RewardItem = GameUITools.CreateMinReward(this.Data.Info.WelfareRewardType, this.Data.Info.WelfareRewardValue1, this.Data.Info.WelfareRewardValue2, this.Reward);
		}
		this.RefreshBtnState();
		base.gameObject.SetActive(true);
	}

	private void RefreshBtnState()
	{
		if (this.Data.IsComplete())
		{
			if (this.Data.IsTakeReward())
			{
				this.bg.spriteName = "Price_bg";
				this.ReceiveBtn.gameObject.SetActive(false);
				this.NoTake.gameObject.SetActive(false);
				this.finished.SetActive(true);
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.ReceiveBtn.gameObject.SetActive(true);
				this.NoTake.gameObject.SetActive(false);
				this.finished.SetActive(false);
			}
		}
		else
		{
			this.bg.spriteName = "gold_bg";
			this.ReceiveBtn.gameObject.SetActive(false);
			this.NoTake.gameObject.SetActive(true);
			this.finished.SetActive(false);
		}
	}
}
