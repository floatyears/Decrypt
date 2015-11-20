using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GuildSearchTabLayer : MonoBehaviour
{
	private UIInput mNameInput;

	private GuildJoinTabItemTable mSearchItemTable;

	private GameObject mTipGo;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mNameInput = base.transform.Find("nameInput").GetComponent<UIInput>();
		GameObject gameObject = base.transform.Find("searchBtn").gameObject;
		UIEventListener expr_37 = UIEventListener.Get(gameObject);
		expr_37.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_37.onClick, new UIEventListener.VoidDelegate(this.OnSearchBtnClick));
		this.mSearchItemTable = base.transform.Find("contentsPanel/recordContents").gameObject.AddComponent<GuildJoinTabItemTable>();
		this.mSearchItemTable.maxPerLine = 1;
		this.mSearchItemTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mSearchItemTable.cellWidth = 880f;
		this.mSearchItemTable.cellHeight = 90f;
		this.mSearchItemTable.scrollBar = base.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
		this.mTipGo = base.transform.Find("tipTxt").gameObject;
		this.mTipGo.SetActive(false);
	}

	public void DoRefreshJoinItems()
	{
		this.mSearchItemTable.ClearData();
		this.InitJoinItems();
	}

	public void Refresh(ulong guildId)
	{
		for (int i = 0; i < this.mSearchItemTable.transform.childCount; i++)
		{
			Transform child = this.mSearchItemTable.transform.GetChild(i);
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
        //GuildSearchTabLayer.<UpdateScrollBar>c__Iterator5C <UpdateScrollBar>c__Iterator5C = new GuildSearchTabLayer.<UpdateScrollBar>c__Iterator5C();
        //<UpdateScrollBar>c__Iterator5C.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator5C;
	}

	private void InitJoinItems()
	{
		if (Globals.Instance.Player.GuildSystem.GuildListForSearch != null && Globals.Instance.Player.GuildSystem.GuildListForSearch.Count > 0)
		{
			for (int i = 0; i < Globals.Instance.Player.GuildSystem.GuildListForSearch.Count; i++)
			{
				this.AddJoinItem(Globals.Instance.Player.GuildSystem.GuildListForSearch[i]);
			}
		}
		base.StartCoroutine(this.UpdateScrollBar());
		this.mTipGo.SetActive(Globals.Instance.Player.GuildSystem.GuildListForSearch.Count == 0);
	}

	private void AddJoinItem(BriefGuildData bfGD)
	{
		this.mSearchItemTable.AddData(new GuildJoinTabItemData(bfGD, false));
	}

	private void OnSearchBtnClick(GameObject go)
	{
		this.mTipGo.SetActive(false);
		if (string.IsNullOrEmpty(this.mNameInput.value))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guild1", 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.mNameInput.value) > 12)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EGR_11", 0f, 0f);
			return;
		}
		MC2S_GetGuildList mC2S_GetGuildList = new MC2S_GetGuildList();
		ulong iD = 0uL;
		if (ulong.TryParse(this.mNameInput.value, out iD))
		{
			mC2S_GetGuildList.ID = iD;
			mC2S_GetGuildList.Name = string.Empty;
		}
		else
		{
			mC2S_GetGuildList.ID = 0uL;
			mC2S_GetGuildList.Name = this.mNameInput.value;
		}
		Globals.Instance.CliSession.Send(909, mC2S_GetGuildList);
	}
}
