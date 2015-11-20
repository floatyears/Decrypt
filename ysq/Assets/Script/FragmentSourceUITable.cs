using System;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSourceUITable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/CommonSourceItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		CommonSourceItem commonSourceItem = gameObject.AddComponent<CommonSourceItem>();
		commonSourceItem.InitWithBaseScene(370);
		return commonSourceItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByIndex));
	}

	private int SortByIndex(BaseData a, BaseData b)
	{
		CommonSourceItemData commonSourceItemData = (CommonSourceItemData)a;
		CommonSourceItemData commonSourceItemData2 = (CommonSourceItemData)b;
		if (commonSourceItemData == null || commonSourceItemData2 == null)
		{
			return 0;
		}
		if (commonSourceItemData.GetID() > commonSourceItemData2.GetID())
		{
			return 1;
		}
		if (commonSourceItemData.GetID() < commonSourceItemData2.GetID())
		{
			return -1;
		}
		return 0;
	}
}
