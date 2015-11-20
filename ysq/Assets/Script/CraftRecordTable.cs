using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecordTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/craftRecordItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		CraftRecordItem craftRecordItem = gameObject.AddComponent<CraftRecordItem>();
		craftRecordItem.InitWithBaseScene();
		return craftRecordItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		CraftRecordItemData craftRecordItemData = (CraftRecordItemData)a;
		CraftRecordItemData craftRecordItemData2 = (CraftRecordItemData)b;
		if (craftRecordItemData == null || craftRecordItemData2 == null)
		{
			return 0;
		}
		if (craftRecordItemData.RecordData.TimeStamp > craftRecordItemData2.RecordData.TimeStamp)
		{
			return -1;
		}
		if (craftRecordItemData.RecordData.TimeStamp < craftRecordItemData2.RecordData.TimeStamp)
		{
			return 1;
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRankLvl));
	}
}
