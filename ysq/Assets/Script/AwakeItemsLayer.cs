using System;
using UnityEngine;

public class AwakeItemsLayer : MonoBehaviour
{
	private GUIPropsBagScene mBaseScene;

	private AwakeItemsBagUITable mContentsTable;

	private GameObject mTips;

	public bool IsInit = true;

	public void InitWithBaseScene(GUIPropsBagScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTips = GameUITools.FindGameObject("Tips", base.gameObject);
		GameUITools.RegisterClickEvent("Go", new UIEventListener.VoidDelegate(this.OnGoClick), this.mTips);
		this.mContentsTable = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<AwakeItemsBagUITable>();
		this.mContentsTable.maxPerLine = 2;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 442f;
		this.mContentsTable.cellHeight = 130f;
		this.mContentsTable.gapHeight = 8f;
		this.mContentsTable.gapWidth = 8f;
		this.mContentsTable.InitWithBaseScene(this.mBaseScene, "GUIAwakeItemsBagItem");
		this.mContentsTable.focusID = GameUIManager.mInstance.uiState.SelectItemID;
		GameUIManager.mInstance.uiState.SelectItemID = 0uL;
	}

	private void OnGoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, false, true);
	}

	private void RefreshTips()
	{
		this.mTips.gameObject.SetActive(this.mContentsTable.mDatas.Count == 0);
	}

	public void Refresh()
	{
		if (this.IsInit)
		{
			this.IsInit = false;
			this.InitPropsBagItems();
		}
	}

	public void AddItem(ItemDataEx data)
	{
		if (this.IsInit)
		{
			return;
		}
		if (data.Info.Type != 5)
		{
			return;
		}
		this.mContentsTable.AddData(data);
		this.RefreshTips();
	}

	public void RemoveItem(ulong id)
	{
		if (this.IsInit)
		{
			return;
		}
		this.mContentsTable.RemoveData(id);
		this.RefreshTips();
	}

	public void UpdataItem(ItemDataEx data)
	{
		if (this.IsInit)
		{
			return;
		}
		if (data.Info.Type != 5)
		{
			return;
		}
		if (this.mContentsTable.gridItems != null)
		{
			for (int i = 0; i < this.mContentsTable.gridItems.Length; i++)
			{
				GUICommonBagItem gUICommonBagItem = (GUICommonBagItem)this.mContentsTable.gridItems[i];
				if (gUICommonBagItem != null && gUICommonBagItem.mData.Data.ID == data.Data.ID)
				{
					gUICommonBagItem.Refresh();
					break;
				}
			}
		}
		this.mContentsTable.repositionNow = true;
		this.RefreshTips();
	}

	private void InitPropsBagItems()
	{
		this.mContentsTable.ClearData();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 5)
			{
				if (!GameUIManager.mInstance.uiState.CachePropsBag)
				{
					current.ClearUIData();
				}
				this.mContentsTable.AddData(current);
			}
		}
		GameUIManager.mInstance.uiState.CachePropsBag = true;
		this.RefreshTips();
	}
}
