using System;
using System.Collections.Generic;
using UnityEngine;

public class PillageFarmUITable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/PillageFarmItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PillageFarmItem pillageFarmItem = gameObject.AddComponent<PillageFarmItem>();
		pillageFarmItem.Init();
		return pillageFarmItem;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	private int Sort(BaseData a, BaseData b)
	{
		PillageFarmItemData pillageFarmItemData = (PillageFarmItemData)a;
		PillageFarmItemData pillageFarmItemData2 = (PillageFarmItemData)b;
		if (pillageFarmItemData != null && pillageFarmItemData2 != null)
		{
			if (pillageFarmItemData.GetID() > pillageFarmItemData2.GetID())
			{
				return 1;
			}
			if (pillageFarmItemData.GetID() < pillageFarmItemData2.GetID())
			{
				return -1;
			}
		}
		return 0;
	}
}
