using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIPVP4FarmPopUp : MonoBehaviour
{
	private GameObject mWinBG;

	private GameObject mCloseBtn;

	private GameObject mFinishBtn;

	private UIPanel mPanel;

	private UITable mTable;

	private List<MS2C_PvpArenaResult> mDatas;

	private UnityEngine.Object prefab;

	public void Init()
	{
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mWinBG = GameUITools.FindGameObject("winBG", base.gameObject);
		this.mCloseBtn = GameUITools.RegisterClickEvent("closeBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWinBG);
		this.mFinishBtn = GameUITools.RegisterClickEvent("finishBtn", new UIEventListener.VoidDelegate(this.OnFinishClick), this.mWinBG);
		this.mPanel = GameUITools.FindGameObject("panel", this.mWinBG).GetComponent<UIPanel>();
		this.mTable = GameUITools.FindGameObject("contents", this.mPanel.gameObject).GetComponent<UITable>();
	}

	public void Show(List<MS2C_PvpArenaResult> datas)
	{
		if (datas == null)
		{
			return;
		}
		this.mDatas = datas;
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.mCloseBtn.SetActive(false);
		this.mFinishBtn.gameObject.SetActive(false);
		this.ClearItems();
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mWinBG.transform, new TweenDelegate.TweenCallback(this.ShowItems), true);
	}

	private void ShowItems()
	{
		base.StartCoroutine(this.PlayAnim());
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim()
	{
        return null;
        //GUIPVP4FarmPopUp.<PlayAnim>c__Iterator81 <PlayAnim>c__Iterator = new GUIPVP4FarmPopUp.<PlayAnim>c__Iterator81();
        //<PlayAnim>c__Iterator.<>f__this = this;
        //return <PlayAnim>c__Iterator;
	}

	private void ClearItems()
	{
		for (int i = 0; i < this.mTable.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.mTable.transform.GetChild(i).gameObject);
		}
	}

	public PVP4FarmItem AddOneItem(int index, MS2C_PvpArenaResult data)
	{
		if (this.prefab == null)
		{
			this.prefab = Res.LoadGUI("GUI/PVP4FarmItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.prefab);
		gameObject.transform.parent = this.mTable.gameObject.transform;
		gameObject.transform.localScale = Vector3.one;
		PVP4FarmItem pVP4FarmItem = gameObject.AddComponent<PVP4FarmItem>();
		pVP4FarmItem.Init(index, data);
		this.mTable.Reposition();
		pVP4FarmItem.ShowAnim();
		return pVP4FarmItem;
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWinBG.transform, new TweenDelegate.TweenCallback(this.CloseAnimEnd), true);
	}

	private void OnFinishClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUITools.PlayCloseWindowAnim(this.mWinBG.transform, new TweenDelegate.TweenCallback(this.CloseAnimEnd), true);
	}

	private void CloseAnimEnd()
	{
		base.gameObject.SetActive(false);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}
}
