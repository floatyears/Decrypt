using System;
using UnityEngine;

public class GuildActivityTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildActiveItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildActivityItem guildActivityItem = gameObject.AddComponent<GuildActivityItem>();
		guildActivityItem.InitWithBaseScene();
		return guildActivityItem;
	}
}
