using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MyDeedItem : UICustomGridItem
{
	private AHDataEx mAHData;

	private UILabel mNum;

	private UILabel mTips;

	private GameObject mReward;

	private GameObject[] RewardItem = new GameObject[3];

	private List<HalloweenContract> conData;

	public void Init()
	{
		this.mNum = base.transform.Find("num").GetComponent<UILabel>();
		this.mReward = base.transform.Find("reward").gameObject;
		this.mTips = base.transform.Find("tips").GetComponent<UILabel>();
	}

	public override void Refresh(object data)
	{
		if (this.mAHData == data)
		{
			return;
		}
		this.mAHData = (AHDataEx)data;
		this.RefreshItem();
	}

	private void RefreshItem()
	{
		if (this.mAHData == null)
		{
			return;
		}
		this.mNum.text = this.mAHData.mNum.ToString();
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		if (this.mAHData.mNotOpen)
		{
			this.mTips.gameObject.SetActive(true);
			this.mTips.text = Singleton<StringManager>.Instance.GetString("festival14_3");
			this.mTips.color = Color.green;
		}
		else if (this.mAHData.mUnlucky)
		{
			this.mTips.gameObject.SetActive(true);
			this.mTips.text = Singleton<StringManager>.Instance.GetString("festival4");
			this.mTips.color = Color.red;
		}
		else if (this.mAHData.mItemID != null)
		{
			this.mTips.text = string.Empty;
			for (int j = 0; j < this.mAHData.mItemID.Count; j++)
			{
				this.RewardItem[j] = GameUITools.CreateReward(this.mAHData.mItemID[j].RewardType, this.mAHData.mItemID[j].RewardValue1, this.mAHData.mItemID[j].RewardValue2, this.mReward.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[j] != null)
				{
					this.RewardItem[j].gameObject.AddComponent<UIDragScrollView>();
					this.RewardItem[j].transform.localScale = new Vector3(0.43f, 0.43f, 0.43f);
					this.RewardItem[j].transform.localPosition = new Vector3((float)(j * 44), 0f, 0f);
				}
			}
		}
	}
}
