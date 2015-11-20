using System;
using System.Collections.Generic;
using UnityEngine;

public class LoginGroupTable : UICustomGrid
{
	protected GUIGameLoginScene mBaseScene;

	public void Init(GUIGameLoginScene basescene)
	{
		this.mBaseScene = basescene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GroupItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUILoginGroupItem gUILoginGroupItem = gameObject.AddComponent<GUILoginGroupItem>();
		gUILoginGroupItem.Init(this.mBaseScene);
		return gUILoginGroupItem;
	}

	public int SortByRank(BaseData a, BaseData b)
	{
		GroupItemInfoData groupItemInfoData = (GroupItemInfoData)a;
		GroupItemInfoData groupItemInfoData2 = (GroupItemInfoData)b;
		if (groupItemInfoData == null || groupItemInfoData2 == null)
		{
			return 0;
		}
		if (groupItemInfoData.GetID() > groupItemInfoData2.GetID())
		{
			return -1;
		}
		if (groupItemInfoData.GetID() < groupItemInfoData2.GetID())
		{
			return 1;
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRank));
	}
}
