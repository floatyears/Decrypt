using System;
using System.Collections.Generic;
using UnityEngine;

public class PVPRecordTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/pvpRecordItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PVPRecordItemPVP4 pVPRecordItemPVP = gameObject.AddComponent<PVPRecordItemPVP4>();
		pVPRecordItemPVP.InitWithBaseScene();
		return pVPRecordItemPVP;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		PVPRecordItemData pVPRecordItemData = (PVPRecordItemData)a;
		PVPRecordItemData pVPRecordItemData2 = (PVPRecordItemData)b;
		if (pVPRecordItemData == null || pVPRecordItemData2 == null)
		{
			return 0;
		}
		if (pVPRecordItemData.RecordData.TimeStamp > pVPRecordItemData2.RecordData.TimeStamp)
		{
			return -1;
		}
		if (pVPRecordItemData.RecordData.TimeStamp < pVPRecordItemData2.RecordData.TimeStamp)
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
