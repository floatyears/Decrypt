using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBoxUITable : UICustomGrid
{
	private ItemsBox mBaseBox;

	public void Init(ItemsBox basebox)
	{
		this.mBaseBox = basebox;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		ItemsBoxItem itemsBoxItem = CommonIconItem.Create(base.gameObject, Vector3.zero, null, true, 0.8f, null).gameObject.AddComponent<ItemsBoxItem>();
		itemsBoxItem.InitWithBaseScene(this.mBaseBox);
		return itemsBoxItem;
	}

	protected override void Sort(List<BaseData> list)
	{
	}
}
