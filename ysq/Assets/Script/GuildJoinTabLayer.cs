using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GuildJoinTabLayer : MonoBehaviour
{
	private GuildJoinTabItemTable mJoinItemsTable;

	private GameObject mTxtTip;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.DoRefreshJoinItems();
	}

	private void CreateObjects()
	{
		this.mTxtTip = base.transform.Find("tipTxt").gameObject;
		this.mJoinItemsTable = base.transform.Find("recordBg/contentsPanel/recordContents").gameObject.AddComponent<GuildJoinTabItemTable>();
		this.mJoinItemsTable.maxPerLine = 1;
		this.mJoinItemsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mJoinItemsTable.cellWidth = 880f;
		this.mJoinItemsTable.cellHeight = 90f;
		this.mJoinItemsTable.scrollBar = base.transform.Find("recordBg/contentsScrollBar").GetComponent<UIScrollBar>();
	}

	public void DoRefreshJoinItems()
	{
		this.mJoinItemsTable.ClearData();
		this.InitJoinItems();
	}

	public void Refresh(ulong guildId)
	{
		for (int i = 0; i < this.mJoinItemsTable.transform.childCount; i++)
		{
			Transform child = this.mJoinItemsTable.transform.GetChild(i);
			GuildJoinTabItem component = child.GetComponent<GuildJoinTabItem>();
			if (component != null && component.mBriefGuildData != null && component.mBriefGuildData.mBriefGuildData != null && component.mBriefGuildData.mBriefGuildData.ID == guildId)
			{
				component.Refresh();
				break;
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GuildJoinTabLayer.<UpdateScrollBar>c__Iterator5B <UpdateScrollBar>c__Iterator5B = new GuildJoinTabLayer.<UpdateScrollBar>c__Iterator5B();
        //<UpdateScrollBar>c__Iterator5B.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator5B;
	}

	private void InitJoinItems()
	{
		List<BriefGuildData> guildList = Globals.Instance.Player.GuildSystem.GuildList;
		if (guildList != null && guildList.Count > 0)
		{
			for (int i = 0; i < guildList.Count; i++)
			{
				this.AddJoinItem(guildList[i]);
			}
			if (guildList.Count == 30)
			{
				this.AddRefreshBtn();
			}
		}
		base.StartCoroutine(this.UpdateScrollBar());
		this.mTxtTip.SetActive(guildList.Count == 0);
	}

	private void AddJoinItem(BriefGuildData bfGD)
	{
		this.mJoinItemsTable.AddData(new GuildJoinTabItemData(bfGD, false));
	}

	private void AddRefreshBtn()
	{
		this.mJoinItemsTable.AddData(new GuildJoinTabItemData(null, true));
	}
}
