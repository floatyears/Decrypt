using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonRankItemTable : UICustomGrid
{
	protected object mBaseScene;

	public string className;

	public void InitWithBaseScene(object baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		if (string.IsNullOrEmpty(this.className))
		{
			global::Debug.Log(new object[]
			{
				"className is null or empty"
			});
			return null;
		}
		string str = "commonRankItem";
		if (this.mBaseScene is BillboardCommonLayer)
		{
			str = "BBCommonRankItem";
		}
		UnityEngine.Object @object = Res.LoadGUI("GUI/" + str);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		CommonRankItemBase commonRankItemBase = gameObject.AddComponent(this.className) as CommonRankItemBase;
		commonRankItemBase.InitWithBaseScene(this.mBaseScene);
		return commonRankItemBase;
	}

	public int SortByRank(BaseData a, BaseData b)
	{
		BillboardInfoData billboardInfoData = (BillboardInfoData)a;
		BillboardInfoData billboardInfoData2 = (BillboardInfoData)b;
		if (billboardInfoData == null || billboardInfoData2 == null)
		{
			return 0;
		}
		if (billboardInfoData.GetID() > billboardInfoData2.GetID())
		{
			return 1;
		}
		if (billboardInfoData.GetID() < billboardInfoData2.GetID())
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
