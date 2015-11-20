using System;
using System.Collections.Generic;
using UnityEngine;

public class AAItemDataGrid : UICustomGrid
{
	private UnityEngine.Object AAItemDataPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private AADataGridItem AddOneTragetItem()
	{
		if (this.AAItemDataPrefab == null)
		{
			this.AAItemDataPrefab = Res.LoadGUI("GUI/AADataGridItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.AAItemDataPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		AADataGridItem aADataGridItem = gameObject.AddComponent<AADataGridItem>();
		aADataGridItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return aADataGridItem;
	}

	private int GetSortWeight(AAItemDataEx data)
	{
		int result = 1000;
		if (data.IsTakeReward())
		{
			result = 0;
		}
		else if (data.IsComplete())
		{
			result = 1000000;
		}
		return result;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		AAItemDataEx aAItemDataEx = (AAItemDataEx)a;
		AAItemDataEx aAItemDataEx2 = (AAItemDataEx)b;
		int sortWeight = this.GetSortWeight(aAItemDataEx);
		int sortWeight2 = this.GetSortWeight(aAItemDataEx2);
		if (sortWeight != sortWeight2)
		{
			return (sortWeight >= sortWeight2) ? -1 : 1;
		}
		if (aAItemDataEx.AAData.ID == aAItemDataEx2.AAData.ID)
		{
			return 0;
		}
		return (aAItemDataEx.AAData.ID >= aAItemDataEx2.AAData.ID) ? 1 : -1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
