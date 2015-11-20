using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGuildMinesRecordTable : UICustomGrid
{
	private GUIGuildMinesRecordPopUp mBaseScene;

	public void Init(GUIGuildMinesRecordPopUp basescene)
	{
		this.mBaseScene = basescene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUIGuildMinesRecordItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGuildMinesRecordItem gUIGuildMinesRecordItem = gameObject.AddComponent<GUIGuildMinesRecordItem>();
		gUIGuildMinesRecordItem.Init(this.mBaseScene);
		return gUIGuildMinesRecordItem;
	}

	private int SortByID(BaseData a, BaseData b)
	{
		GUIGuildMinesRecordData gUIGuildMinesRecordData = (GUIGuildMinesRecordData)a;
		GUIGuildMinesRecordData gUIGuildMinesRecordData2 = (GUIGuildMinesRecordData)b;
		if (gUIGuildMinesRecordData != null && gUIGuildMinesRecordData2 != null)
		{
			if (gUIGuildMinesRecordData.GetID() > gUIGuildMinesRecordData2.GetID())
			{
				return -1;
			}
			if (gUIGuildMinesRecordData.GetID() < gUIGuildMinesRecordData2.GetID())
			{
				return 1;
			}
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByID));
	}
}
