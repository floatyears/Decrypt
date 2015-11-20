using Att;
using System;
using UnityEngine;

public sealed class SevenDayRewardItem : AARewardItemBase
{
	private SevenDayRewardDataEx Data;

	private SevenDayInfo CacheInfo;

	protected override void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null || this.Data.Info == null)
		{
			return;
		}
		AchievementItem.GoAchievement((EAchievementConditionType)this.Data.Info.ConditionType);
	}

	protected override void OnReceiveBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		GUISevenDayRewardScene.RequestTakeSevenDayReward(this.Data);
	}

	public override void Refresh(object _data)
	{
		SevenDayRewardDataEx sevenDayRewardDataEx = (SevenDayRewardDataEx)_data;
		if (sevenDayRewardDataEx == this.Data && this.CacheInfo == sevenDayRewardDataEx.Info)
		{
			this.RefreshFinnishState();
			return;
		}
		this.Data = sevenDayRewardDataEx;
		this.CacheInfo = this.Data.Info;
		this.RefreshData();
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.Data == null || this.Data.Info == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.Title.text = this.Data.Info.Name;
		float num = 254f;
		int num2 = 0;
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		for (int j = 0; j < this.Data.Info.RewardType.Count; j++)
		{
			if (j < this.RewardItem.Length && this.Data.Info.RewardType[j] != 0 && this.Data.Info.RewardType[j] != 20)
			{
				this.RewardItem[num2] = GameUITools.CreateMinReward(this.Data.Info.RewardType[j], this.Data.Info.RewardValue1[j], this.Data.Info.RewardValue2[j], this.Reward);
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
			if (this.Data.Data.TakeReward)
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
			this.GoBtn.SetActive(this.Data.Info.ConditionType != 31);
			this.ReceiveBtn.SetActive(false);
			this.finished.SetActive(false);
		}
		if (this.Data.Info.Value == 0 || !this.Data.Info.ShowProgress)
		{
			this.step.gameObject.SetActive(false);
		}
		else
		{
			this.step.gameObject.SetActive(true);
			this.step.text = string.Format("{0} {1}/{2}", Singleton<StringManager>.Instance.GetString("QuestProgress"), Tools.FormatValue(this.Data.Data.Value), Tools.FormatValue(this.Data.Info.Value));
		}
	}
}
