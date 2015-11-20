using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildApplyTabLayer : MonoBehaviour
{
	private GuildApplyMemberItemTable mApplyItemsTable;

	private GameObject mTipTxt;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTipTxt = base.transform.Find("tipTxt").gameObject;
		this.mApplyItemsTable = base.transform.Find("contentsPanel/recordContents").gameObject.AddComponent<GuildApplyMemberItemTable>();
		this.mApplyItemsTable.maxPerLine = 1;
		this.mApplyItemsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mApplyItemsTable.cellWidth = 880f;
		this.mApplyItemsTable.cellHeight = 76f;
		this.mApplyItemsTable.scrollBar = base.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
	}

	public void DoRefreshApplyItems(List<GuildApplication> applyDatas)
	{
		this.mApplyItemsTable.ClearData();
		this.InitApplyItems(applyDatas);
	}

	private void InitApplyItems(List<GuildApplication> applyDatas)
	{
		for (int i = 0; i < applyDatas.Count; i++)
		{
			this.AddApplyItem(applyDatas[i]);
		}
		this.mTipTxt.SetActive(applyDatas.Count == 0);
	}

	private void AddApplyItem(GuildApplication ga)
	{
		this.mApplyItemsTable.AddData(new GuildApplyMemberData(ga));
	}

	public void RemoveApplyItem(ulong applyId)
	{
		this.mApplyItemsTable.RemoveData(applyId);
	}
}
