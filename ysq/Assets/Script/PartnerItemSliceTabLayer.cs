using System;
using UnityEngine;

public class PartnerItemSliceTabLayer : MonoBehaviour
{
	public GUIPartnerManageScene mBaseScene;

	public BagPartnerSliceInventoryTable bagPartnerInventoryTable;

	private GameObject mTips;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.InitBagInventory();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("bagPanel");
		this.mTips = GameUITools.FindGameObject("Tips", transform.gameObject);
		GameUITools.FindUILabel("Label", GameUITools.RegisterClickEvent("Go", new UIEventListener.VoidDelegate(this.OnGoClick), this.mTips));
		this.bagPartnerInventoryTable = base.transform.Find("bagPanel/bagContents").gameObject.AddComponent<BagPartnerSliceInventoryTable>();
		this.bagPartnerInventoryTable.maxPerLine = 2;
		this.bagPartnerInventoryTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.bagPartnerInventoryTable.cellWidth = 450f;
		this.bagPartnerInventoryTable.cellHeight = 110f;
		this.bagPartnerInventoryTable.gapWidth = 2f;
		this.bagPartnerInventoryTable.gapHeight = 2f;
		this.bagPartnerInventoryTable.InitWithBaseScene(this.mBaseScene);
		this.bagPartnerInventoryTable.focusID = GameUIManager.mInstance.uiState.SelectItemID;
		GameUIManager.mInstance.uiState.SelectItemID = 0uL;
	}

	private void OnGoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResetWMSceneInfo = true;
		GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, true);
	}

	private void RefreshTips()
	{
		this.mTips.gameObject.SetActive(this.bagPartnerInventoryTable.mDatas.Count == 0);
	}

	public void InitBagInventory()
	{
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 3 && current.Info.SubType == 0)
			{
				this.bagPartnerInventoryTable.AddData(current);
			}
		}
		this.bagPartnerInventoryTable.repositionNow = true;
		GameUIManager.mInstance.uiState.CachePetBag = true;
		this.RefreshTips();
	}

	public void AddItem(ItemDataEx data)
	{
		this.bagPartnerInventoryTable.AddData(data);
		this.RefreshTips();
	}

	public void RemoveItem(ulong id)
	{
		this.bagPartnerInventoryTable.RemoveData(id);
		this.RefreshTips();
	}

	public void UpdateItem(ItemDataEx data)
	{
		if (this.bagPartnerInventoryTable.gridItems != null)
		{
			for (int i = 0; i < this.bagPartnerInventoryTable.gridItems.Length; i++)
			{
				BagParInventoryItem bagParInventoryItem = (BagParInventoryItem)this.bagPartnerInventoryTable.gridItems[i];
				if (bagParInventoryItem != null && bagParInventoryItem.mItemDataEx.Data.ID == data.Data.ID)
				{
					bagParInventoryItem.Refresh();
					break;
				}
			}
		}
		this.bagPartnerInventoryTable.repositionNow = true;
		this.RefreshTips();
	}
}
