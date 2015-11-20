using Att;
using System;
using UnityEngine;

public class GUIAwakeItemListPanelItem : MonoBehaviour
{
	private AwakeItemDetailLayer mBaseLayer;

	public ItemInfo mParentInfo;

	public ItemInfo mItemInfo;

	private UISprite mArrow;

	public UISprite mSelect;

	public GUIAwakeItemListPanelItem Init(AwakeItemDetailLayer baselayer, ItemInfo info, ItemInfo parentInfo)
	{
		this.mBaseLayer = baselayer;
		this.mParentInfo = parentInfo;
		this.mItemInfo = info;
		this.mArrow = GameUITools.FindUISprite("Arrow", base.gameObject);
		this.mSelect = GameUITools.FindUISprite("Select", base.gameObject);
		this.mSelect.enabled = false;
		CommonIconItem.Create(base.gameObject, new Vector3(20f, 25f, 0f), new CommonIconItem.VoidCallBack(this.OnItemClick), true, 0.5f, null).Refresh(info, false, false, false);
		if (this.mParentInfo == null)
		{
			this.mArrow.enabled = false;
		}
		else
		{
			this.mArrow.enabled = true;
		}
		return this;
	}

	private void OnItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseLayer.Refresh(this.mItemInfo, this.mParentInfo, false);
	}
}
