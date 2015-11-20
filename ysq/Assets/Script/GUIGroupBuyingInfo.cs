using Proto;
using System;
using UnityEngine;

public class GUIGroupBuyingInfo : MonoBehaviour
{
	private UILabel time1;

	private UILabel mScore;

	private UIButton mRewardBtn;

	private UIButton mRulesBtn;

	private ActivityGroupBuyingGrid mActivityGroupBuyingGrid;

	private float time1Flag = 0.2f;

	public ActivityGroupBuyingData AGBData
	{
		get;
		private set;
	}

	public void Init(ActivityGroupBuyingData data)
	{
		this.CreateObjects();
		this.Refresh(data);
	}

	public void CreateObjects()
	{
		this.mActivityGroupBuyingGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<ActivityGroupBuyingGrid>();
		this.mActivityGroupBuyingGrid.maxPerLine = 1;
		this.mActivityGroupBuyingGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mActivityGroupBuyingGrid.cellWidth = 648f;
		this.mActivityGroupBuyingGrid.cellHeight = 148f;
		this.time1 = base.transform.Find("time").GetComponent<UILabel>();
		this.time1.text = string.Empty;
		this.mScore = base.transform.Find("score").GetComponent<UILabel>();
		this.mScore.text = string.Empty;
		this.mRewardBtn = base.transform.Find("rewardBtn").GetComponent<UIButton>();
		this.mRulesBtn = base.transform.Find("rulesBtn").GetComponent<UIButton>();
		UIEventListener expr_F4 = UIEventListener.Get(this.mRewardBtn.gameObject);
		expr_F4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F4.onClick, new UIEventListener.VoidDelegate(this.OnRewardBtnClick));
		UIEventListener expr_125 = UIEventListener.Get(this.mRulesBtn.gameObject);
		expr_125.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_125.onClick, new UIEventListener.VoidDelegate(this.OnRulesBtnClick));
		MC2S_ActivityGroupBuyingInfo ojb = new MC2S_ActivityGroupBuyingInfo();
		Globals.Instance.CliSession.Send(773, ojb);
	}

	public void Refresh(ActivityGroupBuyingData data)
	{
		if (this.AGBData == data)
		{
			this.time1Flag = 0.1f;
			this.mActivityGroupBuyingGrid.repositionNow = true;
		}
		else
		{
			this.time1Flag = 0.1f;
			this.mActivityGroupBuyingGrid.SetDragAmount(0f, 0f);
			this.mActivityGroupBuyingGrid.ClearData();
			if (data != null)
			{
				for (int i = 0; i < data.Data.Count; i++)
				{
					ActivityGroupBuyingItem activityGroupBuyingItem = data.Data[i];
					if (activityGroupBuyingItem != null)
					{
						this.mActivityGroupBuyingGrid.AddData(new ActivityGroupBuyingDataEx(data, activityGroupBuyingItem));
					}
				}
			}
			this.mActivityGroupBuyingGrid.repositionNow = true;
		}
		this.AGBData = data;
	}

	public void Refresh(int activityID, ActivityGroupBuyingItem data)
	{
		if (this.AGBData == null || this.AGBData.Base.ID != activityID)
		{
			return;
		}
		this.mActivityGroupBuyingGrid.repositionNow = true;
	}

	public void OnActivityGroupBuyingEvent()
	{
		if (this.AGBData == null)
		{
			return;
		}
		this.mActivityGroupBuyingGrid.repositionNow = true;
		this.RefreshTime();
		this.GetScore();
	}

	public void OnGetGroupBuyingDataEvent()
	{
		this.mActivityGroupBuyingGrid.repositionNow = true;
		this.GetScore();
	}

	private void GetScore()
	{
		int gBScore = Globals.Instance.Player.ActivitySystem.GBScore;
		if (gBScore > 0)
		{
			this.mScore.text = Singleton<StringManager>.Instance.GetString("groupBuy_6", new object[]
			{
				gBScore
			});
		}
	}

	public void OnGroupBuyingBuyEvent(ActivityGroupBuyingItem data)
	{
		this.GetScore();
	}

	private void OnRulesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("groupBuy", "groupBuy_1");
	}

	private void OnRewardBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGroupBuyingRewardPopUp.ShowMe();
	}

	private void Update()
	{
		this.RefreshTime();
	}

	private void RefreshTime()
	{
		this.time1Flag -= Time.deltaTime;
		if (this.time1 != null && this.time1Flag < 0f)
		{
			int num = (this.AGBData != null) ? Tools.GetRemainAARewardTime(this.AGBData.Base.CloseTimeStamp) : 0;
			if (num <= 0)
			{
				this.time1.text = Singleton<StringManager>.Instance.GetString("activityOverTime1", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.time1Flag = 3.40282347E+38f;
			}
			else
			{
				this.time1.text = Singleton<StringManager>.Instance.GetString("activityOverTime1", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.time1Flag = 1f;
			}
		}
	}
}
