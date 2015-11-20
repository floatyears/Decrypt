using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildCraftRecord : GameUIBasePopup
{
	private CraftRecordTable mPvpRecordTable;

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildCraftRecord, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("BG").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject2);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mPvpRecordTable = gameObject.transform.Find("recordBg/contentsPanel/recordContents").gameObject.AddComponent<CraftRecordTable>();
		this.mPvpRecordTable.maxPerLine = 1;
		this.mPvpRecordTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mPvpRecordTable.cellWidth = 638f;
		this.mPvpRecordTable.cellHeight = 88f;
		this.mPvpRecordTable.scrollBar = gameObject.transform.Find("recordBg/contentsScrollBar").GetComponent<UIScrollBar>();
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GUIGuildCraftRecord.<UpdateScrollBar>c__Iterator57 <UpdateScrollBar>c__Iterator = new GUIGuildCraftRecord.<UpdateScrollBar>c__Iterator57();
        //<UpdateScrollBar>c__Iterator.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator;
	}

	public override void InitPopUp()
	{
		List<GuildWarBattleRecord> battleRecords = Globals.Instance.Player.GuildSystem.BattleRecords;
		for (int i = 0; i < battleRecords.Count; i++)
		{
			this.mPvpRecordTable.AddData(new CraftRecordItemData(battleRecords[i]));
		}
		base.StartCoroutine(this.UpdateScrollBar());
	}
}
