using Att;
using Proto;
using System;
using UnityEngine;

public class ASPDataGridItem : AARewardItemBase
{
	private ASPItemDataEx Data;

	private UILabel process;

	public override void Init()
	{
		base.Init();
		this.process = base.transform.FindChild("process").GetComponent<UILabel>();
	}

	protected override void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null || this.Data.ASPItem == null)
		{
			return;
		}
		AchievementItem.GoAchievement(EAchievementConditionType.EACT_Pay);
	}

	protected override void OnReceiveBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		if (this.Data.IsComplete())
		{
			return;
		}
		if (!this.Data.IsNotTakeReward())
		{
			return;
		}
		MC2S_TakePayReward mC2S_TakePayReward = new MC2S_TakePayReward();
		mC2S_TakePayReward.ActivityID = this.Data.ASPData.Base.ID;
		mC2S_TakePayReward.ProductID = this.Data.ASPItem.ProductID;
		Globals.Instance.CliSession.Send(768, mC2S_TakePayReward);
	}

	public override void Refresh(object _data)
	{
		if (_data == this.Data)
		{
			this.RefreshFinnishState();
			return;
		}
		this.Data = (ASPItemDataEx)_data;
		this.RefreshData();
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.ASPItem == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.Title.text = this.Data.GetTitle();
		float num = 240f;
		int num2 = 0;
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		for (int j = 0; j < this.Data.ASPItem.Data.Count; j++)
		{
			if (j < this.RewardItem.Length && this.Data.ASPItem.Data[j].RewardType != 0 && this.Data.ASPItem.Data[j].RewardType != 20)
			{
				this.RewardItem[num2] = GameUITools.CreateMinReward(this.Data.ASPItem.Data[j].RewardType, this.Data.ASPItem.Data[j].RewardValue1, this.Data.ASPItem.Data[j].RewardValue2, this.Reward);
				if (this.RewardItem[num2] != null)
				{
					this.RewardItem[num2].transform.localPosition = new Vector3((float)num2 * num, 0f, 0f);
					num2++;
				}
			}
		}
		this.RefreshFinnishState();
		base.gameObject.SetActive(true);
	}

	private void RefreshFinnishState()
	{
		if (this.Data.IsComplete())
		{
			this.GoBtn.SetActive(false);
			this.bg.spriteName = "Price_bg";
			this.ReceiveBtn.SetActive(false);
			this.finished.SetActive(true);
			this.step.gameObject.SetActive(false);
		}
		else if (this.Data.IsNotTakeReward())
		{
			this.GoBtn.SetActive(false);
			this.bg.spriteName = "gold_bg";
			this.ReceiveBtn.SetActive(true);
			this.finished.SetActive(false);
			this.step.gameObject.SetActive(true);
			this.step.text = Singleton<StringManager>.Instance.GetString("ASPT_1", new object[]
			{
				this.Data.ASPItem.PayCount - this.Data.ASPItem.RewardCount
			});
		}
		else
		{
			this.GoBtn.SetActive(true);
			this.bg.spriteName = "gold_bg";
			this.ReceiveBtn.SetActive(false);
			this.finished.SetActive(false);
			if (this.Data.ASPItem.MaxCount > 0)
			{
				this.step.gameObject.SetActive(true);
				this.step.text = Singleton<StringManager>.Instance.GetString("ASPT_1", new object[]
				{
					this.Data.ASPItem.PayCount - this.Data.ASPItem.RewardCount
				});
			}
			else
			{
				this.step.gameObject.SetActive(false);
			}
		}
		if (this.Data.ASPItem.MaxCount > 0)
		{
			this.process.gameObject.SetActive(true);
			int rewardCount = this.Data.ASPItem.RewardCount;
			this.process.text = Singleton<StringManager>.Instance.GetString("ASPT_3", new object[]
			{
				rewardCount,
				this.Data.ASPItem.MaxCount
			});
		}
		else
		{
			this.process.gameObject.SetActive(false);
		}
	}
}
