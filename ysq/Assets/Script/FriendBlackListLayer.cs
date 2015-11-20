using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendBlackListLayer : MonoBehaviour
{
	private UILabel mBlackListCount;

	private UILabel mTxt;

	private FriendCommonGrid mFriendBlackListTable;

	private bool IsInit;

	public void Init()
	{
		this.CreateObjects();
	}

	public void RefreshLayer()
	{
		if (!this.IsInit)
		{
			this.IsInit = true;
			this.InitInventoryItems();
		}
		this.mTxt.enabled = true;
		this.mBlackListCount.enabled = true;
		this.mTxt.text = Singleton<StringManager>.Instance.GetString("friend_10");
		this.mBlackListCount.text = Singleton<StringManager>.Instance.GetString("friend_9", new object[]
		{
			this.mFriendBlackListTable.mDatas.Count,
			GameConst.GetInt32(195)
		});
		this.mBlackListCount.color = Color.yellow;
		this.mFriendBlackListTable.repositionNow = true;
	}

	private void CreateObjects()
	{
		this.mTxt = base.transform.Find("blackListCount").GetComponent<UILabel>();
		this.mBlackListCount = this.mTxt.transform.Find("Label").GetComponent<UILabel>();
		this.mTxt.enabled = false;
		this.mBlackListCount.enabled = false;
		this.mFriendBlackListTable = base.transform.FindChild("blackListPanel/blackListContents").gameObject.AddComponent<FriendCommonGrid>();
		this.mFriendBlackListTable.maxPerLine = 2;
		this.mFriendBlackListTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mFriendBlackListTable.cellWidth = 450f;
		this.mFriendBlackListTable.cellHeight = 120f;
		this.mFriendBlackListTable.gapHeight = 2f;
		this.mFriendBlackListTable.gapWidth = 2f;
		this.mFriendBlackListTable.focusID = GameUIManager.mInstance.uiState.SelectFriendID;
	}

	public void AddFriendItem(FriendData data)
	{
		if (data.GUID == 0uL || data.FriendType != 2)
		{
			return;
		}
		this.mFriendBlackListTable.AddData(new FriendDataEx(data, EUITableLayers.ESL_BlackList, null, null));
		this.RefreshLayer();
	}

	public void RemoveFriendItem(ulong id)
	{
		if (this.mFriendBlackListTable.RemoveData(id))
		{
			this.RefreshLayer();
		}
	}

	public void UpdateFriendItem(ulong id)
	{
		if (id == 0uL || this.mFriendBlackListTable.GetData(id) != null)
		{
			this.RefreshLayer();
		}
	}

	public void InitInventoryItems()
	{
		this.mFriendBlackListTable.ClearData();
		List<FriendData> blackList = Globals.Instance.Player.FriendSystem.blackList;
		for (int i = 0; i < blackList.Count; i++)
		{
			if (blackList[i].GUID != 0uL && blackList[i].FriendType == 2)
			{
				this.mFriendBlackListTable.AddData(new FriendDataEx(blackList[i], EUITableLayers.ESL_BlackList, null, null));
			}
		}
	}
}
