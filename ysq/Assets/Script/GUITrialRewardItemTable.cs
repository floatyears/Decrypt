using System;
using UnityEngine;

public class GUITrialRewardItemTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/trialRewardItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUITrialRewardItem gUITrialRewardItem = gameObject.AddComponent<GUITrialRewardItem>();
		gUITrialRewardItem.InitWithBaseScene();
		return gUITrialRewardItem;
	}
}
