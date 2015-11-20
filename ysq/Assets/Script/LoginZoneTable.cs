using System;
using System.Collections.Generic;
using UnityEngine;

public class LoginZoneTable : UICustomGrid
{
	protected GUIGameLoginScene mBaseScene;

	public void InitWithBaseScene(GUIGameLoginScene baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/ZoneItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUILoginZoneItem gUILoginZoneItem = gameObject.AddComponent<GUILoginZoneItem>();
		gUILoginZoneItem.InitWithBaseScene(this.mBaseScene);
		return gUILoginZoneItem;
	}

	public int SortByRank(BaseData a, BaseData b)
	{
		ZoneItemInfoData zoneItemInfoData = (ZoneItemInfoData)a;
		ZoneItemInfoData zoneItemInfoData2 = (ZoneItemInfoData)b;
		if (zoneItemInfoData == null || zoneItemInfoData2 == null)
		{
			return 0;
		}
		if (zoneItemInfoData.GetID() > zoneItemInfoData2.GetID())
		{
			return 1;
		}
		if (zoneItemInfoData.GetID() < zoneItemInfoData2.GetID())
		{
			return -1;
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRank));
	}
}
