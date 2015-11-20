using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGroupBuyingRewardPopUp : GameUIBasePopup
{
	private UIButton mSureBtn;

	private GUIGroupBuyingRewardTable mGUIGroupBuyingRewardTable;

	public List<RewardData> mRwardDatas = new List<RewardData>();

	private UILabel mTxt;

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGroupBuyingRewardPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.InitRewardItems();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		this.mTxt = transform.Find("flower/Label").GetComponent<UILabel>();
		GameObject gameObject = transform.Find("sureBtn").gameObject;
		this.mSureBtn = gameObject.GetComponent<UIButton>();
		UIEventListener expr_4A = UIEventListener.Get(gameObject);
		expr_4A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4A.onClick, new UIEventListener.VoidDelegate(this.OnOneGetBtnClick));
		GameObject gameObject2 = transform.Find("CloseBtn").gameObject;
		UIEventListener expr_82 = UIEventListener.Get(gameObject2);
		expr_82.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_82.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mGUIGroupBuyingRewardTable = transform.transform.Find("itemsPanel/itemsContents").gameObject.AddComponent<GUIGroupBuyingRewardTable>();
		this.mGUIGroupBuyingRewardTable.maxPerLine = 1;
		this.mGUIGroupBuyingRewardTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUIGroupBuyingRewardTable.cellWidth = 610f;
		this.mGUIGroupBuyingRewardTable.cellHeight = 91f;
		this.mGUIGroupBuyingRewardTable.InitWithBaseScene(this);
		ActivitySubSystem expr_116 = Globals.Instance.Player.ActivitySystem;
		expr_116.GBScoreRewardEvent = (ActivitySubSystem.AGBCallBack1)Delegate.Combine(expr_116.GBScoreRewardEvent, new ActivitySubSystem.AGBCallBack1(this.OnGBScoreRewardEvent));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		ActivitySubSystem expr_20 = Globals.Instance.Player.ActivitySystem;
		expr_20.GBScoreRewardEvent = (ActivitySubSystem.AGBCallBack1)Delegate.Remove(expr_20.GBScoreRewardEvent, new ActivitySubSystem.AGBCallBack1(this.OnGBScoreRewardEvent));
	}

	public void Refresh()
	{
		this.mTxt.text = Singleton<StringManager>.Instance.GetString("groupBuy_8");
		bool flag = false;
		for (int i = 0; i < this.mGUIGroupBuyingRewardTable.mDatas.Count; i++)
		{
			GUIGroupBuyingRewardDataEx gUIGroupBuyingRewardDataEx = (GUIGroupBuyingRewardDataEx)this.mGUIGroupBuyingRewardTable.mDatas[i];
			if (gUIGroupBuyingRewardDataEx != null && gUIGroupBuyingRewardDataEx.IsCanTaken() && !gUIGroupBuyingRewardDataEx.IsTaken())
			{
				flag = true;
				break;
			}
		}
		this.mSureBtn.isEnabled = flag;
		Tools.SetButtonState(this.mSureBtn.gameObject, flag);
	}

	private void InitRewardItems()
	{
		this.mGUIGroupBuyingRewardTable.ClearData();
		for (int i = 0; i < Globals.Instance.Player.ActivitySystem.GBData.ScoreReward.Count; i++)
		{
			this.mGUIGroupBuyingRewardTable.AddData(new GUIGroupBuyingRewardDataEx(Globals.Instance.Player.ActivitySystem.GBData.ScoreReward[i]));
		}
	}

	public void AddRewardData(int rdType, int rdValue1, int rdValue2)
	{
		this.mRwardDatas.Clear();
		this.mRwardDatas.Add(new RewardData
		{
			RewardType = rdType,
			RewardValue1 = rdValue1,
			RewardValue2 = rdValue2
		});
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnOneGetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_ActivityGroupBuyingScoreReward mC2S_ActivityGroupBuyingScoreReward = new MC2S_ActivityGroupBuyingScoreReward();
		mC2S_ActivityGroupBuyingScoreReward.ID = 0;
		Globals.Instance.CliSession.Send(777, mC2S_ActivityGroupBuyingScoreReward);
	}

	public void OnGBScoreRewardEvent(List<int> data)
	{
		this.mRwardDatas.Clear();
		for (int i = 0; i < data.Count; i++)
		{
			ActivityGroupBuyingScoreReward gBSIReward = Globals.Instance.Player.ActivitySystem.GetGBSIReward(data[i]);
			this.mRwardDatas.Add(new RewardData
			{
				RewardType = gBSIReward.Reward.RewardType,
				RewardValue1 = gBSIReward.Reward.RewardValue1,
				RewardValue2 = gBSIReward.Reward.RewardValue2
			});
		}
		this.Refresh();
		if (this.mRwardDatas.Count > 0)
		{
			GUIRewardPanel.Show(this.mRwardDatas, null, false, false, null, false);
		}
	}
}
