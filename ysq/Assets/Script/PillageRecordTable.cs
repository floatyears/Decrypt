using System;
using System.Collections.Generic;
using UnityEngine;

public class PillageRecordTable : UICustomGrid
{
	private UnityEngine.Object PillageRecordItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private PillageRecordItem AddOneTragetItem()
	{
		if (this.PillageRecordItemPrefab == null)
		{
			this.PillageRecordItemPrefab = Res.LoadGUI("GUI/PillageRecordItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.PillageRecordItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PillageRecordItem pillageRecordItem = gameObject.AddComponent<PillageRecordItem>();
		pillageRecordItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return pillageRecordItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		PillageRecordItemData pillageRecordItemData = (PillageRecordItemData)a;
		PillageRecordItemData pillageRecordItemData2 = (PillageRecordItemData)b;
		if (pillageRecordItemData == null || pillageRecordItemData2 == null)
		{
			return 0;
		}
		if (pillageRecordItemData.RecordData.TimeStamp > pillageRecordItemData2.RecordData.TimeStamp)
		{
			return -1;
		}
		if (pillageRecordItemData.RecordData.TimeStamp < pillageRecordItemData2.RecordData.TimeStamp)
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
