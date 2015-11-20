using System;
using System.Collections.Generic;
using UnityEngine;

public class LopetSetBagUITable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUILopetSetBagItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		return gameObject.AddComponent<GUILopetSetBagItem>();
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}

	private int Sort(BaseData a, BaseData b)
	{
		return 0;
	}
}
