using System;
using System.Collections.Generic;
using UnityEngine;

public class PetViewUITable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		GUIPetViewItem gUIPetViewItem = CommonIconItem.Create(base.gameObject, Vector3.zero, null, true, 1f, null).gameObject.AddComponent<GUIPetViewItem>();
		gUIPetViewItem.InitWithBaseScene();
		return gUIPetViewItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	private int Sort(BaseData a, BaseData b)
	{
		PetViewItemData petViewItemData = (PetViewItemData)a;
		PetViewItemData petViewItemData2 = (PetViewItemData)b;
		if (petViewItemData == null || petViewItemData2 == null)
		{
			return 0;
		}
		if (petViewItemData.info.Quality > petViewItemData2.info.Quality)
		{
			return -1;
		}
		if (petViewItemData.info.Quality < petViewItemData2.info.Quality)
		{
			return 1;
		}
		if (petViewItemData.info.ID < petViewItemData2.info.ID)
		{
			return -1;
		}
		if (petViewItemData.info.ID > petViewItemData2.info.ID)
		{
			return 1;
		}
		return 0;
	}
}
