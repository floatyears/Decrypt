using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LuckyDeedItem : UICustomGridItem
{
	private const int MAX_REWARD = 3;

	private ActivityHalloweenDataEx mAHData;

	private UILabel mNumTxt;

	private UILabel mNum;

	private UILabel mRewardTxt;

	private GameObject mReward;

	private GameObject[] RewardItem = new GameObject[3];

	private List<HalloweenContract> conData;

	private GameObject mbg;

	private UILabel mTips;

	private UILabel mTips2;

	public void Init()
	{
		this.mNumTxt = base.transform.Find("numTxt").GetComponent<UILabel>();
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
		this.mTips = base.transform.Find("tips").GetComponent<UILabel>();
		this.mTips.gameObject.SetActive(false);
		this.mTips2 = base.transform.Find("tips2").GetComponent<UILabel>();
		this.mTips2.gameObject.SetActive(false);
		this.mReward = base.transform.Find("infoBg/reward").gameObject;
		this.mRewardTxt = base.transform.Find("rewardTxt").GetComponent<UILabel>();
		this.mbg = base.transform.Find("infoBg").gameObject;
	}

	public override void Refresh(object data)
	{
		if (this.mAHData == data)
		{
			return;
		}
		this.mAHData = (ActivityHalloweenDataEx)data;
		this.RefreshItem();
	}

	private void RefreshItem()
	{
		if (this.mAHData == null)
		{
			return;
		}
		if (this.mAHData.mProductID == 1)
		{
			this.mNumTxt.text = Singleton<StringManager>.Instance.GetString("festival7");
			this.mRewardTxt.text = Singleton<StringManager>.Instance.GetString("festival9");
		}
		else if (this.mAHData.mProductID == 2)
		{
			this.mNumTxt.text = Singleton<StringManager>.Instance.GetString("festival6");
			this.mRewardTxt.text = Singleton<StringManager>.Instance.GetString("festival10");
		}
		else if (this.mAHData.mProductID == 3)
		{
			this.mNumTxt.text = Singleton<StringManager>.Instance.GetString("festival8");
			this.mRewardTxt.text = Singleton<StringManager>.Instance.GetString("festival11");
		}
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		if (this.mAHData.mItemID != null)
		{
			this.mbg.SetActive(true);
			for (int j = 0; j < this.mAHData.mItemID.Count; j++)
			{
				this.RewardItem[j] = GameUITools.CreateReward(this.mAHData.mItemID[j].RewardType, this.mAHData.mItemID[j].RewardValue1, this.mAHData.mItemID[j].RewardValue2, this.mReward.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[j] != null)
				{
					this.mNum.text = Singleton<StringManager>.Instance.GetString("festival13", new object[]
					{
						this.mAHData.mLuckyNum,
						this.mAHData.mPlayerName
					});
					this.RewardItem[j].gameObject.AddComponent<UIDragScrollView>();
					this.RewardItem[j].transform.localScale = new Vector3(0.43f, 0.43f, 0.43f);
					this.RewardItem[j].transform.localPosition = new Vector3((float)(j * 44), 0f, 0f);
				}
			}
		}
		else
		{
			this.mTips.gameObject.SetActive(true);
			this.mTips2.gameObject.SetActive(true);
			this.mTips.text = Singleton<StringManager>.Instance.GetString("festival14_3");
			this.mTips2.text = Singleton<StringManager>.Instance.GetString("festival14_3");
			this.mbg.SetActive(false);
		}
	}
}
