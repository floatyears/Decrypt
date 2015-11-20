using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityGroupBuyingGrid : UICustomGrid
{
	private UnityEngine.Object AAItemDataPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private GUIGroupBuyingItem AddOneTragetItem()
	{
		if (this.AAItemDataPrefab == null)
		{
			this.AAItemDataPrefab = Res.LoadGUI("GUI/GUIGroupBuyingItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.AAItemDataPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGroupBuyingItem gUIGroupBuyingItem = gameObject.AddComponent<GUIGroupBuyingItem>();
		gUIGroupBuyingItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return gUIGroupBuyingItem;
	}

	private int GetSortWeight(ActivityGroupBuyingDataEx data)
	{
		int result;
		if (data.IsBuy())
		{
			result = 1000000;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		ActivityGroupBuyingDataEx activityGroupBuyingDataEx = (ActivityGroupBuyingDataEx)a;
		ActivityGroupBuyingDataEx activityGroupBuyingDataEx2 = (ActivityGroupBuyingDataEx)b;
		int sortWeight = this.GetSortWeight(activityGroupBuyingDataEx);
		int sortWeight2 = this.GetSortWeight(activityGroupBuyingDataEx2);
		if (sortWeight != sortWeight2)
		{
			return (sortWeight >= sortWeight2) ? -1 : 1;
		}
		if (activityGroupBuyingDataEx.mGUIGroupBuyingItem.ID == activityGroupBuyingDataEx2.mGUIGroupBuyingItem.ID)
		{
			return 0;
		}
		return (activityGroupBuyingDataEx.mGUIGroupBuyingItem.ID >= activityGroupBuyingDataEx2.mGUIGroupBuyingItem.ID) ? 1 : -1;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
