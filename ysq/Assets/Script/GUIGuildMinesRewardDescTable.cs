using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGuildMinesRewardDescTable : UICustomGrid
{
	private int width;

	private bool showBtns;

	public void Init(int width = 610, bool showBtns = false)
	{
		this.width = width;
		this.showBtns = showBtns;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUIGuildMinesRewardDescItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGuildMinesRewardDescItem gUIGuildMinesRewardDescItem = gameObject.AddComponent<GUIGuildMinesRewardDescItem>();
		gUIGuildMinesRewardDescItem.Init(this.width, this.showBtns);
		return gUIGuildMinesRewardDescItem;
	}

	public void Refresh(int ID)
	{
		if (ID == 0)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GUIGuildMinesRewardDescItem component = base.transform.GetChild(i).GetComponent<GUIGuildMinesRewardDescItem>();
				if (component != null)
				{
					component.Refresh();
				}
			}
		}
		else
		{
			for (int j = 0; j < base.transform.childCount; j++)
			{
				GUIGuildMinesRewardDescItem component2 = base.transform.GetChild(j).GetComponent<GUIGuildMinesRewardDescItem>();
				if (component2 != null && component2.mData.mInfo.ID == ID)
				{
					component2.Refresh();
				}
			}
		}
		base.repositionNow = true;
	}

	private int SortByID(BaseData a, BaseData b)
	{
		GUIGuildMinesRewardDescData gUIGuildMinesRewardDescData = (GUIGuildMinesRewardDescData)a;
		GUIGuildMinesRewardDescData gUIGuildMinesRewardDescData2 = (GUIGuildMinesRewardDescData)b;
		if (gUIGuildMinesRewardDescData != null && gUIGuildMinesRewardDescData2 != null)
		{
			bool flag = gUIGuildMinesRewardDescData.IsTaken();
			bool flag2 = gUIGuildMinesRewardDescData2.IsTaken();
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag && flag2)
			{
				return -1;
			}
			bool flag3 = gUIGuildMinesRewardDescData.CanTake();
			bool flag4 = gUIGuildMinesRewardDescData2.CanTake();
			if (flag3 && !flag4)
			{
				return -1;
			}
			if (!flag3 && flag4)
			{
				return 1;
			}
			if (gUIGuildMinesRewardDescData.GetID() > gUIGuildMinesRewardDescData2.GetID())
			{
				return 1;
			}
			if (gUIGuildMinesRewardDescData.GetID() < gUIGuildMinesRewardDescData2.GetID())
			{
				return -1;
			}
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByID));
	}
}
