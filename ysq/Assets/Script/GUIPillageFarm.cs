using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIPillageFarm : MonoBehaviour
{
	private Transform mWinBG;

	private GameObject mCloseBtn;

	private UIPanel panel;

	private PillageFarmUITable mFarmTable;

	private GameObject mFinishFarm;

	private bool isPlaying;

	public void Init()
	{
		base.transform.localPosition = new Vector3(0f, 0f, -580f);
		this.mWinBG = base.transform.Find("winBG");
		this.mCloseBtn = this.mWinBG.Find("closeBtn").gameObject;
		this.mCloseBtn.SetActive(false);
		UIEventListener expr_67 = UIEventListener.Get(this.mCloseBtn);
		expr_67.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_67.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		UIEventListener expr_93 = UIEventListener.Get(base.gameObject);
		expr_93.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_93.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		this.mFinishFarm = this.mWinBG.Find("finish").gameObject;
		this.mFinishFarm.SetActive(false);
		UIEventListener expr_E6 = UIEventListener.Get(this.mFinishFarm);
		expr_E6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E6.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		UIEventListener expr_121 = UIEventListener.Get(base.transform.Find("FadeBG").gameObject);
		expr_121.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_121.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		this.panel = this.mWinBG.FindChild("bagPanel").GetComponent<UIPanel>();
		this.mFarmTable = this.panel.transform.Find("bagContents").gameObject.AddComponent<PillageFarmUITable>();
		this.mFarmTable.maxPerLine = 1;
		this.mFarmTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mFarmTable.cellWidth = 486f;
		this.mFarmTable.cellHeight = 130f;
		this.mFarmTable.gapHeight = 0f;
		this.mFarmTable.gapWidth = 0f;
	}

	public void Show(List<MS2C_PvpPillageResult> data)
	{
		base.gameObject.SetActive(true);
		this.mFarmTable.ClearData();
		base.StartCoroutine(this.RefreshPillageResult(data));
	}

	[DebuggerHidden]
	private IEnumerator RefreshPillageResult(List<MS2C_PvpPillageResult> data)
	{
        return null;
        //GUIPillageFarm.<RefreshPillageResult>c__Iterator84 <RefreshPillageResult>c__Iterator = new GUIPillageFarm.<RefreshPillageResult>c__Iterator84();
        //<RefreshPillageResult>c__Iterator.data = data;
        //<RefreshPillageResult>c__Iterator.<$>data = data;
        //<RefreshPillageResult>c__Iterator.<>f__this = this;
        //return <RefreshPillageResult>c__Iterator;
	}

	private void OnCloseBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.isPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		this.mFarmTable.ClearData();
		base.gameObject.SetActive(false);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}
}
