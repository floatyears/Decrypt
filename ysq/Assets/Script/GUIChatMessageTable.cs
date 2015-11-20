using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIChatMessageTable : UICustomGrid
{
	private GUIChatWindowV2 mBaseScene;

	public void InitWithBaseScene(GUIChatWindowV2 baseScene)
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
		GUIChatLineV2 gUIChatLineV = gameObject2.AddComponent<GUIChatLineV2>();
		gUIChatLineV.InitWithBaseScene(this.mBaseScene);
		return gUIChatLineV;
	}

	protected override void Sort(List<BaseData> list)
	{
	}

	public void Refresh()
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUIChatMessageData gUIChatMessageData = (GUIChatMessageData)this.mDatas[i];
			if (gUIChatMessageData != null && gUIChatMessageData.mIsVoice)
			{
				gUIChatMessageData.Refresh();
			}
		}
		for (int j = 0; j < base.transform.childCount; j++)
		{
			GUIChatLineV2 component = base.transform.GetChild(j).GetComponent<GUIChatLineV2>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}
}
