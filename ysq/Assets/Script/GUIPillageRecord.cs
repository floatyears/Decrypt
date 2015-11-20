using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIPillageRecord : MonoBehaviour
{
	private Transform mWinBG;

	private PillageRecordTable mTargetTable;

	public void Show()
	{
		LocalPlayer player = Globals.Instance.Player;
		this.Show(player.PvpSystem.PillageRecord);
	}

	public void Init()
	{
		base.transform.localPosition = new Vector3(0f, 0f, -550f);
		this.mWinBG = base.transform.Find("winBG");
		this.mTargetTable = this.mWinBG.FindChild("bagPanel/bagContents").gameObject.AddComponent<PillageRecordTable>();
		this.mTargetTable.maxPerLine = 1;
		this.mTargetTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mTargetTable.cellWidth = 486f;
		this.mTargetTable.cellHeight = 108f;
		GameObject gameObject = this.mWinBG.FindChild("closeBtn").gameObject;
		UIEventListener expr_A9 = UIEventListener.Get(gameObject);
		expr_A9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A9.onClick, new UIEventListener.VoidDelegate(this.OnCloseTargetList));
		UIEventListener expr_D5 = UIEventListener.Get(base.gameObject);
		expr_D5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D5.onClick, new UIEventListener.VoidDelegate(this.OnCloseTargetList));
	}

	private void OnCloseTargetList(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.ClearTargets();
		base.gameObject.SetActive(false);
	}

	private void Show(List<PillageRecord> Targets)
	{
		this.ClearTargets();
		for (int i = 0; i < Targets.Count; i++)
		{
			this.mTargetTable.AddData(new PillageRecordItemData(Targets[i]));
		}
		base.gameObject.SetActive(true);
	}

	private void ClearTargets()
	{
		this.mTargetTable.ClearData();
	}
}
