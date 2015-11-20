using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityShopGrid : UICustomGrid
{
	public string GridItemPrefabName;

	private UnityEngine.Object AAItemDataPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private UIActivityShopItem AddOneTragetItem()
	{
		if (this.AAItemDataPrefab == null)
		{
			this.AAItemDataPrefab = Res.LoadGUI(this.GridItemPrefabName);
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.AAItemDataPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		UIActivityShopItem uIActivityShopItem = gameObject.AddComponent<UIActivityShopItem>();
		uIActivityShopItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return uIActivityShopItem;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		ActivityShopDataEx activityShopDataEx = (ActivityShopDataEx)a;
		ActivityShopDataEx activityShopDataEx2 = (ActivityShopDataEx)b;
		return activityShopDataEx.AShopItem.ID.CompareTo(activityShopDataEx2.AShopItem.ID);
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
