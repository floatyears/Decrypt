using Att;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIGroupBuyingRewardItem : UICustomGridItem
{
	private GUIGroupBuyingRewardPopUp mBaseScene;

	private UILabel mTitle;

	private UILabel mJinDuNum;

	private UIButton mTakeBtn;

	private GameObject mTakedBtn;

	private UIButton[] mTakeBtns;

	private Transform mReward;

	private GameObject mRewardItem;

	private ItemInfo mItemInfo;

	private GameUIToolTip mToolTips;

	private GUIGroupBuyingRewardDataEx mRewardData;

	private StringBuilder mSb = new StringBuilder(42);

	public GUIGroupBuyingRewardDataEx GetRewgeardData()
	{
		return this.mRewardData;
	}

	public void InitWithBaseScene(GUIGroupBuyingRewardPopUp baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
		ActivitySubSystem expr_1C = Globals.Instance.Player.ActivitySystem;
		expr_1C.GBScoreRewardEvent = (ActivitySubSystem.AGBCallBack1)Delegate.Combine(expr_1C.GBScoreRewardEvent, new ActivitySubSystem.AGBCallBack1(this.OnGBScoreRewardEvent));
	}

	private void OnDestroy()
	{
		ActivitySubSystem expr_0F = Globals.Instance.Player.ActivitySystem;
		expr_0F.GBScoreRewardEvent = (ActivitySubSystem.AGBCallBack1)Delegate.Remove(expr_0F.GBScoreRewardEvent, new ActivitySubSystem.AGBCallBack1(this.OnGBScoreRewardEvent));
	}

	private void CreateObjects()
	{
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mJinDuNum = base.transform.Find("jinDu").GetComponent<UILabel>();
		this.mReward = base.transform.Find("reward");
		this.mTakeBtn = base.transform.Find("takeBtn").GetComponent<UIButton>();
		UIEventListener expr_77 = UIEventListener.Get(this.mTakeBtn.gameObject);
		expr_77.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_77.onClick, new UIEventListener.VoidDelegate(this.OnTakeBtnClick));
		this.mTakeBtns = this.mTakeBtn.GetComponents<UIButton>();
		this.mTakedBtn = base.transform.Find("takedBtn").gameObject;
	}

	public override void Refresh(object data)
	{
		if (this.mRewardData == data)
		{
			this.RefreshFinishedState();
			return;
		}
		this.mRewardData = (GUIGroupBuyingRewardDataEx)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mRewardData != null)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("groupBuy_5", new object[]
			{
				this.mRewardData.mRewardData.Score
			});
			if (this.mRewardItem != null)
			{
				UnityEngine.Object.Destroy(this.mRewardItem);
				this.mRewardItem = null;
			}
			this.mRewardItem = GameUITools.CreateMinReward(this.mRewardData.mRewardData.Reward.RewardType, this.mRewardData.mRewardData.Reward.RewardValue1, this.mRewardData.mRewardData.Reward.RewardValue2, this.mReward);
			this.RefreshFinishedState();
			int gBScore = Globals.Instance.Player.ActivitySystem.GBScore;
			int score = this.mRewardData.mRewardData.Score;
			this.mJinDuNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("QuestProgress")).Append(gBScore).Append("/").Append(score).ToString();
		}
	}

	private void RefreshFinishedState()
	{
		if (this.mRewardData == null)
		{
			return;
		}
		if (this.mRewardData.IsTaken())
		{
			this.mTakeBtn.gameObject.SetActive(false);
			this.mTakedBtn.SetActive(true);
			this.mTakedBtn.transform.localPosition = new Vector3(this.mTakedBtn.transform.localPosition.x, -44f, 0f);
			this.mJinDuNum.gameObject.SetActive(false);
		}
		else
		{
			this.mTakeBtn.gameObject.SetActive(true);
			this.mTakedBtn.SetActive(false);
			this.mJinDuNum.gameObject.SetActive(true);
			bool flag = this.mRewardData.IsCanTaken();
			this.mTakeBtn.isEnabled = flag;
			for (int i = 0; i < this.mTakeBtns.Length; i++)
			{
				this.mTakeBtns[i].SetState((!flag) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
			}
		}
		this.mBaseScene.Refresh();
	}

	private void OnTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mRewardData.IsCanTaken() && this.mRewardData.mRewardData.Score <= Globals.Instance.Player.ActivitySystem.GBScore)
		{
			this.mBaseScene.AddRewardData(this.mRewardData.mRewardData.Reward.RewardType, this.mRewardData.mRewardData.Reward.RewardValue1, this.mRewardData.mRewardData.Reward.RewardValue2);
			MC2S_ActivityGroupBuyingScoreReward mC2S_ActivityGroupBuyingScoreReward = new MC2S_ActivityGroupBuyingScoreReward();
			mC2S_ActivityGroupBuyingScoreReward.ID = this.mRewardData.mRewardData.ID;
			Globals.Instance.CliSession.Send(777, mC2S_ActivityGroupBuyingScoreReward);
		}
	}

	public void OnGBScoreRewardEvent(List<int> data)
	{
		this.RefreshFinishedState();
		this.mBaseScene.Refresh();
	}
}
