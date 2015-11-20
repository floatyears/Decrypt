using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequestLayer : MonoBehaviour
{
	private FriendCommonGrid mFriendRequestTable;

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
		this.mFriendRequestTable.repositionNow = true;
	}

	private void CreateObjects()
	{
		this.mFriendRequestTable = base.transform.FindChild("feiendRequestPanel/friendRequestContents").gameObject.AddComponent<FriendCommonGrid>();
		this.mFriendRequestTable.maxPerLine = 2;
		this.mFriendRequestTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mFriendRequestTable.cellWidth = 450f;
		this.mFriendRequestTable.cellHeight = 120f;
		this.mFriendRequestTable.gapHeight = 2f;
		this.mFriendRequestTable.gapWidth = 2f;
		this.mFriendRequestTable.focusID = GameUIManager.mInstance.uiState.SelectFriendID;
	}

	public void AddFriendItem(FriendData data)
	{
		if (data.GUID == 0uL || data.FriendType != 3)
		{
			return;
		}
		this.mFriendRequestTable.AddData(new FriendDataEx(data, EUITableLayers.ESL_FriendRequest, null, null));
		this.RefreshLayer();
	}

	public void RemoveFriendItem(ulong id)
	{
		if (this.mFriendRequestTable.RemoveData(id))
		{
			this.RefreshLayer();
		}
	}

	public void UpdateFriendItem(ulong id)
	{
		if (id == 0uL || this.mFriendRequestTable.GetData(id) != null)
		{
			this.RefreshLayer();
		}
	}

	public void InitInventoryItems()
	{
		this.mFriendRequestTable.ClearData();
		List<FriendData> applyList = Globals.Instance.Player.FriendSystem.applyList;
		for (int i = 0; i < applyList.Count; i++)
		{
			if (applyList[i].GUID != 0uL && applyList[i].FriendType == 3)
			{
				this.mFriendRequestTable.AddData(new FriendDataEx(applyList[i], EUITableLayers.ESL_FriendRequest, null, null));
			}
		}
		this.RefreshLayer();
	}
}
