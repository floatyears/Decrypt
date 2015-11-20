using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameUIMapFarm : MonoBehaviour
{
	public delegate void MapFarmFinishCallback();

	public GameUIMapFarm.MapFarmFinishCallback MapFarmFinishEvent;

	private Transform mWinBG;

	private GameObject mCloseBtn;

	private GameObject mFarmCollider;

	private UITable mMapFarmTable;

	private bool isPlaying;

	private UnityEngine.Object mapFarmItemPrefab;

	public GameUIAdventureReady mBaseScene
	{
		get;
		private set;
	}

	public void Init(GameUIAdventureReady advReady, List<MS2C_PveResult> data)
	{
		this.mBaseScene = advReady;
		base.transform.localPosition = new Vector3(0f, 0f, -550f);
		this.mWinBG = base.transform.Find("winBG");
		this.mCloseBtn = this.mWinBG.Find("closeBtn").gameObject;
		this.mCloseBtn.SetActive(false);
		UIEventListener expr_6E = UIEventListener.Get(this.mCloseBtn);
		expr_6E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6E.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		UIEventListener expr_A9 = UIEventListener.Get(base.transform.Find("FadeBG").gameObject);
		expr_A9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A9.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		this.mFarmCollider = this.mWinBG.FindChild("FarmCollider").gameObject;
		this.mFarmCollider.SetActive(true);
		this.mMapFarmTable = this.mWinBG.FindChild("bagPanel/bagContents").gameObject.GetComponent<UITable>();
		this.mMapFarmTable.columns = 1;
		this.isPlaying = false;
		GameUIManager.mInstance.uiState.PlayerEnergy -= this.mBaseScene.sceneInfo.CostValue * data.Count;
		base.StartCoroutine(this.RefreshMapFarmItems(data));
	}

	[DebuggerHidden]
	private IEnumerator RefreshMapFarmItems(List<MS2C_PveResult> data)
	{
        return null;
        //GameUIMapFarm.<RefreshMapFarmItems>c__IteratorA1 <RefreshMapFarmItems>c__IteratorA = new GameUIMapFarm.<RefreshMapFarmItems>c__IteratorA1();
        //<RefreshMapFarmItems>c__IteratorA.data = data;
        //<RefreshMapFarmItems>c__IteratorA.<$>data = data;
        //<RefreshMapFarmItems>c__IteratorA.<>f__this = this;
        //return <RefreshMapFarmItems>c__IteratorA;
	}

	public MapFarmItem AddOneItem(int index, MS2C_PveResult pveData)
	{
		if (this.mapFarmItemPrefab == null)
		{
			this.mapFarmItemPrefab = Res.LoadGUI("GUI/mapFarmItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mapFarmItemPrefab);
		gameObject.transform.parent = this.mMapFarmTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MapFarmItem mapFarmItem = gameObject.AddComponent<MapFarmItem>();
		mapFarmItem.InitMapFarmItem(this, index, pveData, this.mBaseScene.sceneInfo);
		this.mMapFarmTable.Reposition();
		mapFarmItem.ShowMapFarmItemAnim(index);
		return mapFarmItem;
	}

	private void OnCloseBtnClicked(GameObject go)
	{
		this.CloseSelf();
	}

	private void CloseSelf()
	{
		if (this.isPlaying)
		{
			return;
		}
		base.StopAllCoroutines();
		GameUITools.PlayCloseWindowAnim(this.mWinBG, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}
}
