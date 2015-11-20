using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIPvpRecordPopUp : GameUIBasePopup
{
	private PVPRecordTable mPvpRecordTable;

	private void Awake()
	{
		this.CreateObjects();
	}

	public override void InitPopUp()
	{
		List<PvpRecord> arenaRecord = Globals.Instance.Player.PvpSystem.ArenaRecord;
		for (int i = 0; i < arenaRecord.Count; i++)
		{
			this.mPvpRecordTable.AddData(new PVPRecordItemData(arenaRecord[i]));
		}
		base.StartCoroutine(this.UpdateScrollBar());
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("BG").gameObject;
		GameObject gameObject2 = gameObject.transform.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject2);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mPvpRecordTable = gameObject.transform.Find("recordBg/contentsPanel/recordContents").gameObject.AddComponent<PVPRecordTable>();
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
        //GUIPvpRecordPopUp.<UpdateScrollBar>c__Iterator8C <UpdateScrollBar>c__Iterator8C = new GUIPvpRecordPopUp.<UpdateScrollBar>c__Iterator8C();
        //<UpdateScrollBar>c__Iterator8C.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator8C;
	}
}
