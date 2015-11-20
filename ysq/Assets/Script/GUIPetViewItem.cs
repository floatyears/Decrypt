using System;
using UnityEngine;

public class GUIPetViewItem : UICustomGridItem
{
	private PetViewItemData mData;

	private PetDataEx tempData;

	private CommonIconItem mIconItem;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIconItem = base.gameObject.GetComponent<CommonIconItem>();
		CommonIconItem expr_17 = this.mIconItem;
		expr_17.OnIconClickEvent = (CommonIconItem.VoidCallBack)Delegate.Combine(expr_17.OnIconClickEvent, new CommonIconItem.VoidCallBack(this.OnIconClick));
	}

	private void OnIconClick(GameObject go)
	{
		GameUIManager.mInstance.ShowPetInfo(this.mData.info);
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (PetViewItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mIconItem.Refresh(this.mData.info, true, false, false);
	}
}
