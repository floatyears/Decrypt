using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendCommonGrid : UICustomGrid
{
	private UnityEngine.Object FriendItemPrefab;

	private GUIFriendScene friendData;

	protected override UICustomGridItem CreateGridItem()
	{
		if (this.FriendItemPrefab == null)
		{
			this.FriendItemPrefab = Res.LoadGUI("GUI/FriendItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.FriendItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		FriendCommonItem friendCommonItem = gameObject.AddComponent<FriendCommonItem>();
		friendCommonItem.Init();
		return friendCommonItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		if (this.friendData == null)
		{
			this.friendData = GameUIManager.mInstance.GetSession<GUIFriendScene>();
		}
		if (this.friendData.currentTable == EUITableLayers.ESL_FriendRecommend || this.friendData.currentTable == EUITableLayers.ESL_Friend)
		{
			list.Sort(new Comparison<BaseData>(this.Sort));
		}
	}

	protected int Sort(BaseData a, BaseData b)
	{
		FriendDataEx aItem = (FriendDataEx)a;
		FriendDataEx bItem = (FriendDataEx)b;
		return this.SortOnLine(aItem, bItem);
	}

	private int SortOnLine(FriendDataEx aItem, FriendDataEx bItem)
	{
		if (aItem == null || bItem == null)
		{
			return 0;
		}
		if (aItem.FriendData.Offline == 0)
		{
			if (bItem.FriendData.Offline == 0)
			{
				return this.SortByCombat(aItem, bItem);
			}
			return -1;
		}
		else
		{
			if (bItem.FriendData.Offline == 0)
			{
				return 1;
			}
			return this.SortByOffLine(aItem, bItem);
		}
	}

	public int SortByCombat(FriendDataEx aItem, FriendDataEx bItem)
	{
		if (aItem.FriendData.CombatValue > bItem.FriendData.CombatValue)
		{
			return -1;
		}
		if (aItem.FriendData.CombatValue < bItem.FriendData.CombatValue)
		{
			return 1;
		}
		return 0;
	}

	public int SortByOffLine(FriendDataEx aItem, FriendDataEx bItem)
	{
		int offline = aItem.FriendData.Offline;
		int num = Globals.Instance.Player.GetTimeStamp() - offline;
		int offline2 = bItem.FriendData.Offline;
		int num2 = Globals.Instance.Player.GetTimeStamp() - offline2;
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}
}
