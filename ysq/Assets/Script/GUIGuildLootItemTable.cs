using System;
using UnityEngine;

public class GUIGuildLootItemTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildRewardItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGuildLootItem gUIGuildLootItem = gameObject.AddComponent<GUIGuildLootItem>();
		gUIGuildLootItem.InitWithBaseScene();
		return gUIGuildLootItem;
	}
}
