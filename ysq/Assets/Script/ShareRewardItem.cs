using Att;
using Proto;
using System;
using UnityEngine;

public class ShareRewardItem : MonoBehaviour
{
	private ShareAchievementDataEx shareData;

	private UISprite mBg;

	private UILabel mTitleTxt;

	private Transform mRewarditem;

	private GameObject mShareRewardBtn;

	private UILabel mRewardBtnTxt;

	private GameObject mShareRewardGet;

	private GameObject mShareRewardNotGet;

	public ShareAchievementDataEx GetShareData()
	{
		return this.shareData;
	}

	public void InitWithBaseScene(ShareAchievementDataEx data)
	{
		this.shareData = data;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.Find("bg").GetComponent<UISprite>();
		this.mTitleTxt = this.mBg.transform.Find("Title").GetComponent<UILabel>();
		this.mRewarditem = this.mBg.transform.Find("panel/reward");
		this.mShareRewardBtn = this.mBg.transform.Find("ToGet").gameObject;
		UIEventListener expr_81 = UIEventListener.Get(this.mShareRewardBtn);
		expr_81.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_81.onClick, new UIEventListener.VoidDelegate(this.OnShareRewardClick));
		this.mRewardBtnTxt = this.mShareRewardBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mShareRewardGet = this.mBg.transform.Find("Get").gameObject;
		this.mShareRewardNotGet = this.mBg.transform.Find("NotGet").gameObject;
	}

	public void Refresh()
	{
		ShareAchievementData data = this.shareData.Data;
		ShareAchievementInfo info = this.shareData.Info;
		this.mTitleTxt.text = info.Name;
		GameUITools.CreateMinReward(2, info.RewardDiamond, 1, this.mRewarditem);
		bool flag = this.shareData.IsComplete();
		this.mBg.spriteName = ((!data.Shared || !data.TakeReward) ? "gold_bg" : "Price_bg");
		this.mShareRewardBtn.SetActive(flag && (!data.Shared || !data.TakeReward));
		if (this.mShareRewardBtn.activeSelf)
		{
			if (!data.Shared)
			{
				this.mRewardBtnTxt.text = Singleton<StringManager>.Instance.GetString("shareTxt5");
			}
			else
			{
				this.mRewardBtnTxt.text = Singleton<StringManager>.Instance.GetString("takeGoods");
			}
		}
		this.mShareRewardGet.SetActive(flag && data.Shared && data.TakeReward);
		this.mShareRewardNotGet.SetActive(!flag);
	}

	private void OnShareRewardClick(GameObject go)
	{
		ShareAchievementData data = this.shareData.Data;
		if (!data.Shared)
		{
			this.ShareGo();
		}
		else
		{
			MC2S_TakeShareAchievementReward mC2S_TakeShareAchievementReward = new MC2S_TakeShareAchievementReward();
			mC2S_TakeShareAchievementReward.ID = data.ID;
			Globals.Instance.CliSession.Send(733, mC2S_TakeShareAchievementReward);
		}
	}

	private void ShareGo()
	{
		if (!GameUISharePopUp.isSharing)
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUISharePopUp, false, null, null);
			GameUISharePopUp gameUISharePopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUISharePopUp;
			gameUISharePopUp.Refresh(this.shareData, false);
		}
	}
}
