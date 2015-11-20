using System;
using System.Collections.Generic;
using UnityEngine;

public class ASPItemDataGrid : UICustomGrid
{
	private UnityEngine.Object ActivitySpecifyPayItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private ASPDataGridItem AddOneTragetItem()
	{
		if (this.ActivitySpecifyPayItemPrefab == null)
		{
			this.ActivitySpecifyPayItemPrefab = Res.LoadGUI("GUI/ASPDataGridItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ActivitySpecifyPayItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		ASPDataGridItem aSPDataGridItem = gameObject.AddComponent<ASPDataGridItem>();
		aSPDataGridItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return aSPDataGridItem;
	}

	private int GetSortWeight(ASPItemDataEx data)
	{
		if (data.IsNotTakeReward())
		{
			return 1000000;
		}
		if (data.IsComplete())
		{
			return 0;
		}
		return 1000;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		ASPItemDataEx aSPItemDataEx = (ASPItemDataEx)a;
		ASPItemDataEx aSPItemDataEx2 = (ASPItemDataEx)b;
		int sortWeight = this.GetSortWeight(aSPItemDataEx);
		int sortWeight2 = this.GetSortWeight(aSPItemDataEx2);
		if (sortWeight != sortWeight2)
		{
			return (sortWeight <= sortWeight2) ? 1 : -1;
		}
		if (aSPItemDataEx.ASPItem.ProductID == aSPItemDataEx2.ASPItem.ProductID)
		{
			return 0;
		}
		return (aSPItemDataEx.ASPItem.ProductID >= aSPItemDataEx2.ASPItem.ProductID) ? 1 : -1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
