using System;
using UnityEngine;

public class GUITrailBranchItemTable : UICustomGrid
{
	private GUITrailTowerSceneV2 mBaseScene;

	public void InitWithBaseScene(GUITrailTowerSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/trainBranchItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUITrailBranchItem gUITrailBranchItem = gameObject.AddComponent<GUITrailBranchItem>();
		gUITrailBranchItem.InitWithBaseScene(this.mBaseScene);
		return gUITrailBranchItem;
	}

	public void Refresh()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GUITrailBranchItem component = base.transform.GetChild(i).GetComponent<GUITrailBranchItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}
}
