using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AwakeItemListPanel : MonoBehaviour
{
	private AwakeItemDetailLayer mBaseLayer;

	private UIGrid mContent;

	private List<GUIAwakeItemListPanelItem> mItems = new List<GUIAwakeItemListPanelItem>();

	private bool isInit = true;

	public void Init(AwakeItemDetailLayer baselayer)
	{
		this.mBaseLayer = baselayer;
		this.mContent = GameUITools.FindGameObject("Content0", base.gameObject).GetComponent<UIGrid>();
		if (this.mContent != null)
		{
			this.mContent.arrangement = UIGrid.Arrangement.Horizontal;
			this.mContent.cellWidth = 90f;
			this.mContent.maxPerLine = 0;
			this.mContent.animateSmoothly = false;
			this.mContent.keepWithinPanel = false;
		}
	}

	private void AddItem(ItemInfo info, ItemInfo parentInfo)
	{
		if (this.mItems.Count > 0)
		{
			this.mItems[this.mItems.Count - 1].mSelect.enabled = false;
		}
		this.mItems.Add(Tools.InstantiateGUIPrefab("GUI/GUIAwakeItemListPanelItem").AddComponent<GUIAwakeItemListPanelItem>().Init(this.mBaseLayer, info, parentInfo));
		GameUITools.AddChild(this.mContent.gameObject, this.mItems[this.mItems.Count - 1].gameObject);
		this.mItems[this.mItems.Count - 1].mSelect.enabled = true;
		this.mContent.Reposition();
		base.Invoke("SetOffset", 0.01f);
	}

	private void SetOffset()
	{
		if (this.isInit)
		{
			this.isInit = false;
			base.GetComponent<UIPanel>().transform.localPosition = new Vector3(-32f, 170f, 0f);
			base.GetComponent<UIPanel>().clipOffset = new Vector2(32f, 0f);
		}
	}

	public void Refresh(ItemInfo info, ItemInfo parentInfo)
	{
		foreach (GUIAwakeItemListPanelItem current in this.mItems)
		{
			if (current.mItemInfo.ID == info.ID)
			{
				this.ChangeTo(current);
				return;
			}
		}
		this.AddItem(info, parentInfo);
	}

	private void ChangeTo(GUIAwakeItemListPanelItem item)
	{
		int num = this.mItems.IndexOf(item);
		while (this.mItems.Count > num + 1)
		{
			UnityEngine.Object.Destroy(this.mItems[this.mItems.Count - 1].gameObject);
			this.mItems.Remove(this.mItems[this.mItems.Count - 1]);
		}
		this.mItems[this.mItems.Count - 1].mSelect.enabled = true;
		if (this.mItems.Count == 1 && Globals.Instance.Player.ItemSystem.GetItemCount(this.mItems[0].mItemInfo.ID) > 0)
		{
			this.mBaseLayer.mBaseScene.SwitchType(GUIAwakeItemInfoPopUp.EOpenType.EOT_Equip, false);
			this.mBaseLayer.mBaseScene.PlayAnim(false, false);
		}
		this.mContent.repositionNow = true;
	}

	public void CheckIfEnough(ItemInfo info)
	{
		foreach (GUIAwakeItemListPanelItem current in this.mItems)
		{
			if (current.mItemInfo.ID == info.ID)
			{
				if (current.mParentInfo != null && Globals.Instance.Player.ItemSystem.GetAwakeItemCreateCount(current.mItemInfo.ID, current.mParentInfo.ID) <= Globals.Instance.Player.ItemSystem.GetItemCount(info.ID))
				{
					this.CheckIfEnough(current.mParentInfo);
					break;
				}
				this.ChangeTo(current);
				this.mBaseLayer.Refresh(current.mItemInfo, current.mParentInfo, false);
				break;
			}
		}
	}
}
