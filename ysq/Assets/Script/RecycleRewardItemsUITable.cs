using System;
using System.Collections.Generic;
using UnityEngine;

public class RecycleRewardItemsUITable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		GUIRecycleGetItem gUIRecycleGetItem = CommonIconItem.Create(base.gameObject, Vector3.zero, null, false, 0.9f, null).gameObject.AddComponent<GUIRecycleGetItem>();
		gUIRecycleGetItem.InitWithBaseScene();
		return gUIRecycleGetItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	protected int Sort(BaseData a, BaseData b)
	{
		RecycleGetItemData recycleGetItemData = (RecycleGetItemData)a;
		RecycleGetItemData recycleGetItemData2 = (RecycleGetItemData)b;
		if (recycleGetItemData == null || recycleGetItemData2 == null)
		{
			return 0;
		}
		if (recycleGetItemData.GetID() > recycleGetItemData2.GetID())
		{
			return 1;
		}
		if (recycleGetItemData.GetID() < recycleGetItemData2.GetID())
		{
			return -1;
		}
		return 0;
	}
}
