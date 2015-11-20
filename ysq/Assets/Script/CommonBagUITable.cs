using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonBagUITable : UICustomGrid
{
	private object mBaseScene;

	private string className;

	public void InitWithBaseScene(object baseScene, string className)
	{
		this.mBaseScene = baseScene;
		this.className = className;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUICommonBagItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUICommonBagItem gUICommonBagItem = gameObject.AddComponent(this.className) as GUICommonBagItem;
		gUICommonBagItem.InitWithBaseScene(this.mBaseScene);
		return gUICommonBagItem;
	}

	protected abstract int Sort(BaseData a, BaseData b);

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.Sort));
	}
}
