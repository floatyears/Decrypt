using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIRewardLevelInfo : MonoBehaviour
{
	private bool mIsInit;

	private bool levelFlag;

	private UITable mLevelRecordTable;

	private UIScrollView mLevelSW;

	private UIScrollBar mLevelScrollBar;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLevelSW = base.transform.Find("rewardPanel").GetComponent<UIScrollView>();
		this.mLevelRecordTable = this.mLevelSW.transform.Find("rewardContents").GetComponent<UITable>();
		this.mLevelRecordTable.columns = 1;
		this.mLevelRecordTable.direction = UITable.Direction.Down;
		this.mLevelRecordTable.sorting = UITable.Sorting.None;
		this.mLevelRecordTable.hideInactive = true;
		this.mLevelRecordTable.keepWithinPanel = true;
		this.mLevelRecordTable.padding = new Vector2(0f, 2f);
		this.mLevelScrollBar = base.transform.Find("scrollBar").GetComponent<UIScrollBar>();
	}

	public void Show()
	{
		if (!this.mIsInit)
		{
			this.mIsInit = true;
			base.StartCoroutine(this.InitRewardItems());
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void RefreshWindow()
	{
		this.mLevelRecordTable.repositionNow = true;
	}

	[DebuggerHidden]
	public IEnumerator InitRewardItems()
	{
        return null;
        //GUIRewardLevelInfo.<InitRewardItems>c__Iterator2F <InitRewardItems>c__Iterator2F = new GUIRewardLevelInfo.<InitRewardItems>c__Iterator2F();
        //<InitRewardItems>c__Iterator2F.<>f__this = this;
        //return <InitRewardItems>c__Iterator2F;
	}

	public void AddLevelReward(MiscInfo mInfo)
	{
		RewardLine rewardLine = this.AddReward(this.mLevelRecordTable.transform);
		if (rewardLine != null)
		{
			rewardLine.Init(RewardLine.ERewardItemType.ERIT_Level, mInfo, null, this.mLevelSW);
		}
	}

	private RewardLine AddReward(Transform parent)
	{
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/Reward");
		if (gameObject == null)
		{
			return null;
		}
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		return gameObject.AddComponent<RewardLine>();
	}

	public void UpdateLevelScrollBar()
	{
		if (!this.levelFlag)
		{
			this.levelFlag = true;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
			{
				if (current.Level > 0)
				{
					num++;
					if (num2 == 0 && !Globals.Instance.Player.IsLevelRewardTaken(current.ID) && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)current.Level))
					{
						num2 = num;
					}
					if (num3 == 0 && !Globals.Instance.Player.IsLevelRewardTaken(current.ID) && (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)current.Level))
					{
						num3 = num;
					}
				}
			}
			if (num2 == 0)
			{
				num2 = num3;
			}
			if (num2 >= 2 && num > 4)
			{
				this.mLevelScrollBar.value = (float)(num2 - 2) / (float)(num - 4);
			}
		}
	}

	public RewardLine GetLevelReward(int id)
	{
		for (int i = 0; i < this.mLevelRecordTable.transform.childCount; i++)
		{
			RewardLine component = this.mLevelRecordTable.transform.GetChild(i).GetComponent<RewardLine>();
			if (component != null && component.GetInfoID() == id)
			{
				return component;
			}
		}
		return null;
	}

	public static bool CanTakeReward()
	{
		foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
		{
			if (current.Level > 0)
			{
				if (!Globals.Instance.Player.IsLevelRewardTaken(current.ID) && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)current.Level))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsAllRewardTaked()
	{
		foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
		{
			if (current.Level > 0)
			{
				if (!Globals.Instance.Player.IsLevelRewardTaken(current.ID))
				{
					return false;
				}
			}
		}
		return true;
	}
}
