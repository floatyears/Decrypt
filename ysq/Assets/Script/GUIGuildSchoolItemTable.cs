using System;
using UnityEngine;

public class GUIGuildSchoolItemTable : UICustomGrid
{
	private GUIGuildSchoolPopUp mBaseScene;

	public void InitWithBaseScene(GUIGuildSchoolPopUp baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildSchoolItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGuildSchoolItem gUIGuildSchoolItem = gameObject.AddComponent<GUIGuildSchoolItem>();
		gUIGuildSchoolItem.InitWithBaseScene(this.mBaseScene);
		return gUIGuildSchoolItem;
	}
}
