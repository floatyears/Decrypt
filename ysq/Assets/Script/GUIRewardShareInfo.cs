using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRewardShareInfo : MonoBehaviour
{
	private bool mIsInit;

	private UITable mShareInfoTable;

	private UIScrollView mShareSW;

	private ShareRewardItem[] rewardItems;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mShareSW = base.transform.Find("rewardPanel").GetComponent<UIScrollView>();
		this.mShareInfoTable = this.mShareSW.transform.Find("rewardContents").gameObject.AddComponent<RewardShareUITable>();
		this.mShareInfoTable.columns = 1;
		this.mShareInfoTable.direction = UITable.Direction.Down;
		this.mShareInfoTable.sorting = UITable.Sorting.Custom;
		this.mShareInfoTable.hideInactive = true;
		this.mShareInfoTable.keepWithinPanel = true;
		this.mShareInfoTable.padding = new Vector2(0f, 2f);
	}

	public void Show()
	{
		if (!this.mIsInit)
		{
			this.mIsInit = true;
			this.InitRewardItems();
		}
		this.RefreshSharePanel();
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void RefreshWindow()
	{
		this.mShareInfoTable.repositionNow = true;
	}

	public void InitRewardItems()
	{
		List<ShareAchievementDataEx> shareAchievements = Globals.Instance.Player.ActivitySystem.ShareAchievements;
		this.rewardItems = new ShareRewardItem[shareAchievements.Count];
		for (int i = 0; i < shareAchievements.Count; i++)
		{
			ShareAchievementDataEx data = shareAchievements[i];
			this.rewardItems[i] = this.AddShareReward(data);
		}
	}

	public ShareRewardItem AddShareReward(ShareAchievementDataEx data)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/ShareRewardItem");
		gameObject.transform.parent = this.mShareInfoTable.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		ShareRewardItem shareRewardItem = gameObject.AddComponent<ShareRewardItem>();
		shareRewardItem.InitWithBaseScene(data);
		return shareRewardItem;
	}

	public void RefreshSharePanel()
	{
		for (int i = 0; i < this.rewardItems.Length; i++)
		{
			ShareRewardItem shareRewardItem = this.rewardItems[i];
			shareRewardItem.Refresh();
		}
		this.mShareInfoTable.repositionNow = true;
	}

	public static bool CanTakeReward()
	{
		if (!GUIRewardShareInfo.IsOpen())
		{
			return false;
		}
		List<ShareAchievementDataEx> shareAchievements = Globals.Instance.Player.ActivitySystem.ShareAchievements;
		for (int i = 0; i < shareAchievements.Count; i++)
		{
			ShareAchievementDataEx shareAchievementDataEx = shareAchievements[i];
			ShareAchievementData data = shareAchievementDataEx.Data;
			bool flag = shareAchievementDataEx.IsComplete();
			if (flag && (!data.Shared || !data.TakeReward))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsOpen()
	{
		return Globals.Instance.Player.IsFunctionEnable(1);
	}
}
