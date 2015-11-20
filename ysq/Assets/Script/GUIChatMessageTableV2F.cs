using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIChatMessageTableV2F : UICustomGrid
{
	private GUIChatWindowV2F mBaseScene;

	public void InitWithBaseScene(GUIChatWindowV2F baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		GameObject gameObject = Res.LoadGUI("GUI/GUIChatLineV2");
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		gameObject2.name = gameObject.name;
		gameObject2.transform.parent = base.gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		GUIChatLineV2F gUIChatLineV2F = gameObject2.AddComponent<GUIChatLineV2F>();
		gUIChatLineV2F.InitWithBaseScene(this.mBaseScene);
		return gUIChatLineV2F;
	}

	protected override void Sort(List<BaseData> list)
	{
	}
}
