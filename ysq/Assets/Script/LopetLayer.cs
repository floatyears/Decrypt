using Proto;
using System;
using UnityEngine;

public class LopetLayer : MonoBehaviour
{
	private GUILopetBagScene mBaseScene;

	private LopetBagUITable mContent;

	private GameObject mTips;

	private bool IsInit = true;

	public void Init(GUILopetBagScene basescene)
	{
		this.mBaseScene = basescene;
	}

	private void CreateObjects()
	{
		this.mTips = GameUITools.FindGameObject("Tips", base.gameObject);
		GameUITools.RegisterClickEvent("Go", new UIEventListener.VoidDelegate(this.OnGoClick), this.mTips);
		this.mContent = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<LopetBagUITable>();
		this.mContent.maxPerLine = 2;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 442f;
		this.mContent.cellHeight = 130f;
		this.mContent.gapHeight = 8f;
		this.mContent.gapWidth = 8f;
		this.mContent.InitWithBaseScene(this.mBaseScene, "GUILopetBagItem");
		this.mContent.focusID = GameUIManager.mInstance.uiState.SelectLopetID;
		GameUIManager.mInstance.uiState.SelectLopetID = 0uL;
	}

	private void OnGoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShopScene.TryOpen(EShopType.EShop_Lopet);
	}

	private void RefreshTips()
	{
		this.mTips.gameObject.SetActive(this.mContent.mDatas.Count == 0);
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.CreateObjects();
			this.IsInit = false;
			this.InitBagItems();
		}
	}

	public void AddItem(LopetDataEx data)
	{
		if (this.IsInit)
		{
			return;
		}
		this.mContent.AddData(data);
		this.RefreshTips();
	}

	public void RemoveItem(ulong id)
	{
		if (this.IsInit)
		{
			return;
		}
		this.mContent.RemoveData(id);
		this.RefreshTips();
	}

	private void InitBagItems()
	{
		this.mContent.ClearData();
		foreach (LopetDataEx current in Globals.Instance.Player.LopetSystem.Values)
		{
			if (!GameUIManager.mInstance.uiState.CacheLopetBag)
			{
				current.ClearUIData();
			}
			this.mContent.AddData(current);
		}
		GameUIManager.mInstance.uiState.CacheLopetBag = true;
		this.RefreshTips();
	}
}
