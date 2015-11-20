using System;
using System.Collections.Generic;
using UnityEngine;

public class VIPRewardGrid : UICustomGrid
{
	private UnityEngine.Object VIPRewardItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private VIPPayRewardItem AddOneTragetItem()
	{
		if (this.VIPRewardItemPrefab == null)
		{
			this.VIPRewardItemPrefab = Res.LoadGUI("GUI/VIPRewardItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.VIPRewardItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		VIPPayRewardItem vIPPayRewardItem = gameObject.AddComponent<VIPPayRewardItem>();
		vIPPayRewardItem.Init();
		return vIPPayRewardItem;
	}

	private int SortBy(BaseData a, BaseData b)
	{
		VIPRewardData vIPRewardData = (VIPRewardData)a;
		VIPRewardData vIPRewardData2 = (VIPRewardData)b;
		return vIPRewardData.GetSortIndex().CompareTo(vIPRewardData2.GetSortIndex());
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortBy));
	}
}
