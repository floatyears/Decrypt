using System;
using UnityEngine;

public class GUIGroupBuyingRewardTable : UICustomGrid
{
	private GUIGroupBuyingRewardPopUp mBaseScene;

	public void InitWithBaseScene(GUIGroupBuyingRewardPopUp baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/GUIGroupBuyingRewardItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGroupBuyingRewardItem gUIGroupBuyingRewardItem = gameObject.AddComponent<GUIGroupBuyingRewardItem>();
		gUIGroupBuyingRewardItem.InitWithBaseScene(this.mBaseScene);
		return gUIGroupBuyingRewardItem;
	}

	public void Refresh(int ID)
	{
		if (ID == 0)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GUIGroupBuyingRewardItem component = base.transform.GetChild(i).GetComponent<GUIGroupBuyingRewardItem>();
				if (component != null)
				{
					component.Refresh();
				}
			}
		}
		base.repositionNow = true;
	}
}
