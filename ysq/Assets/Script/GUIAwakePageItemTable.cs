using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIAwakePageItemTable : UICustomGrid
{
	private GUIAwakeRoadSceneV2 mBaseScene;

	public void InitWithBaseScene(GUIAwakeRoadSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/awakePageItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIAwakePageItem gUIAwakePageItem = gameObject.AddComponent<GUIAwakePageItem>();
		gUIAwakePageItem.InitWithBaseScene(this.mBaseScene);
		return gUIAwakePageItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		GUIAwakePageItemData gUIAwakePageItemData = (GUIAwakePageItemData)a;
		GUIAwakePageItemData gUIAwakePageItemData2 = (GUIAwakePageItemData)b;
		if (gUIAwakePageItemData != null && gUIAwakePageItemData2 != null)
		{
			return gUIAwakePageItemData.mPageIndex - gUIAwakePageItemData2.mPageIndex;
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRankLvl));
	}

	public void RefreshTabState()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GUIAwakePageItem component = base.transform.GetChild(i).GetComponent<GUIAwakePageItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}

	public void SetPageIndex(int index)
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUIAwakePageItemData gUIAwakePageItemData = (GUIAwakePageItemData)this.mDatas[i];
			if (gUIAwakePageItemData != null)
			{
				gUIAwakePageItemData.mIsChecked = (index == gUIAwakePageItemData.mPageIndex);
			}
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			GUIAwakePageItem component = base.transform.GetChild(j).GetComponent<GUIAwakePageItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}
}
