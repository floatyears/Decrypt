using System;
using System.Collections.Generic;
using UnityEngine;

public class APItemDataGrid : UICustomGrid
{
	private UnityEngine.Object APItemDataPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private APSDataGridItem AddOneTragetItem()
	{
		if (this.APItemDataPrefab == null)
		{
			this.APItemDataPrefab = Res.LoadGUI("GUI/ActivityPayShopItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.APItemDataPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		APSDataGridItem aPSDataGridItem = gameObject.AddComponent<APSDataGridItem>();
		aPSDataGridItem.Init();
		return aPSDataGridItem;
	}

	private int GetSortWeight(APItemDataEx data)
	{
		int result = 1000;
		if (data.BuyCount() <= 0)
		{
			result = 0;
		}
		else if (data.APData.OffPrice == 1)
		{
			result = 1000000000;
		}
		else if (data.IsComplete())
		{
			result = 1000000;
		}
		return result;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		APItemDataEx aPItemDataEx = (APItemDataEx)a;
		APItemDataEx aPItemDataEx2 = (APItemDataEx)b;
		int sortWeight = this.GetSortWeight(aPItemDataEx);
		int sortWeight2 = this.GetSortWeight(aPItemDataEx2);
		if (sortWeight != sortWeight2)
		{
			return (sortWeight >= sortWeight2) ? -1 : 1;
		}
		if (aPItemDataEx.APData.ID == aPItemDataEx2.APData.ID)
		{
			return 0;
		}
		return (aPItemDataEx.APData.ID >= aPItemDataEx2.APData.ID) ? 1 : -1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
