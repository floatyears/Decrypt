using Att;
using System;
using UnityEngine;

public class GUIWBRuleInfoRewardItem : MonoBehaviour
{
	private UILabel mRank;

	private UILabel mGemNum;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("Label");
		if (transform != null)
		{
			this.mRank = transform.GetComponent<UILabel>();
		}
		this.mGemNum = base.transform.Find("Gem/num").GetComponent<UILabel>();
	}

	public void Refresh(WorldBossInfo bossInfo)
	{
		if (bossInfo != null)
		{
			int highRank = bossInfo.HighRank;
			int lowRank = bossInfo.LowRank;
			if (lowRank == highRank)
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRank", new object[]
				{
					lowRank
				});
			}
			else
			{
				this.mRank.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRuleRank", new object[]
				{
					string.Format("{0}~{1}", lowRank, highRank)
				});
			}
			if (bossInfo.RewardType[0] == 2)
			{
				this.mGemNum.text = bossInfo.RewardValue1[0].ToString();
			}
			else
			{
				this.mGemNum.text = "0";
			}
		}
	}
}
