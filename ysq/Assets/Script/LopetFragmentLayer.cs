using Proto;
using System;
using UnityEngine;

public class LopetFragmentLayer : MonoBehaviour
{
	private GUILopetBagScene mBaseScene;

	private LopetFragmentBagUITable mContent;

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
		this.mContent = GameUITools.FindGameObject("Contents", base.gameObject).AddComponent<LopetFragmentBagUITable>();
		this.mContent.maxPerLine = 2;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 442f;
		this.mContent.cellHeight = 106f;
		this.mContent.gapHeight = 8f;
		this.mContent.gapWidth = 8f;
		this.mContent.InitWithBaseScene(this.mBaseScene, "GUILopetFragmentBagItem");
		this.mContent.focusID = GameUIManager.mInstance.uiState.SelectItemID;
		GameUIManager.mInstance.uiState.SelectItemID = 0uL;
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

	public void AddItem(ItemDataEx data)
	{
		if (this.IsInit)
		{
			return;
		}
		if (!Tools.IsLopetFragment(data))
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

	public void UpdataItem(ItemDataEx data)
	{
		if (this.IsInit)
		{
			return;
		}
		if (!Tools.IsLopetFragment(data))
		{
			return;
		}
		if (this.mContent.gridItems != null)
		{
			for (int i = 0; i < this.mContent.gridItems.Length; i++)
			{
				GUICommonBagItem gUICommonBagItem = (GUICommonBagItem)this.mContent.gridItems[i];
				if (gUICommonBagItem != null && gUICommonBagItem.mData.GetID() == data.GetID())
				{
					gUICommonBagItem.Refresh();
					break;
				}
			}
		}
		this.RefreshTips();
	}

	private void InitBagItems()
	{
		this.mContent.ClearData();
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (Tools.IsLopetFragment(current))
			{
				if (!GameUIManager.mInstance.uiState.CacheLopetBag)
				{
					current.ClearUIData();
				}
				this.mContent.AddData(current);
			}
		}
		GameUIManager.mInstance.uiState.CacheLopetBag = true;
		this.RefreshTips();
	}
}
