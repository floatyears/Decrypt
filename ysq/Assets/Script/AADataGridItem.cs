using Att;
using Proto;
using System;
using UnityEngine;

public class AADataGridItem : AARewardItemBase
{
	private AAItemDataEx Data;

	protected override void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null || this.Data.AAData == null)
		{
			return;
		}
		AchievementItem.GoAchievement((EAchievementConditionType)this.Data.AAData.Type);
	}

	protected override void OnReceiveBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		if (this.Data.IsTakeReward())
		{
			return;
		}
		if (!this.Data.IsComplete())
		{
			return;
		}
		MC2S_TakeAAReward mC2S_TakeAAReward = new MC2S_TakeAAReward();
		mC2S_TakeAAReward.ActivityID = this.Data.AA.Base.ID;
		mC2S_TakeAAReward.AAItemID = this.Data.AAData.ID;
		Globals.Instance.CliSession.Send(719, mC2S_TakeAAReward);
	}

	public override void Refresh(object _data)
	{
		if (_data == this.Data)
		{
			this.RefreshFinnishState();
			return;
		}
		this.Data = (AAItemDataEx)_data;
		this.RefreshData();
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.AAData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.Title.text = this.Data.GetTitle();
		float num = 246f;
		int num2 = 0;
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		for (int j = 0; j < this.Data.AAData.Data.Count; j++)
		{
			if (j < this.RewardItem.Length && this.Data.AAData.Data[j].RewardType != 0 && this.Data.AAData.Data[j].RewardType != 20)
			{
				this.RewardItem[num2] = GameUITools.CreateMinReward(this.Data.AAData.Data[j].RewardType, this.Data.AAData.Data[j].RewardValue1, this.Data.AAData.Data[j].RewardValue2, this.Reward);
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
			if (this.Data.IsTakeReward())
			{
				this.bg.spriteName = "Price_bg";
				this.ReceiveBtn.SetActive(false);
				this.finished.SetActive(true);
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.ReceiveBtn.SetActive(true);
				this.finished.SetActive(false);
			}
		}
		else
		{
			this.bg.spriteName = "gold_bg";
			this.GoBtn.SetActive(this.Data.AAData.Type != 31);
			this.ReceiveBtn.SetActive(false);
			this.finished.SetActive(false);
		}
		if (this.Data.AAData.Value == 0)
		{
			this.step.gameObject.SetActive(false);
		}
		else
		{
			this.step.gameObject.SetActive(true);
			this.step.text = string.Format("{0} {1}/{2}", Singleton<StringManager>.Instance.GetString("QuestProgress"), this.Data.AAData.CurValue, this.Data.AAData.Value);
		}
	}
}
