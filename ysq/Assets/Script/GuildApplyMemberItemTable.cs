using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildApplyMemberItemTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildApplyRequestItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildApplyMemberItem guildApplyMemberItem = gameObject.AddComponent<GuildApplyMemberItem>();
		guildApplyMemberItem.InitWithBaseScene();
		return guildApplyMemberItem;
	}

	protected override void Sort(List<BaseData> list)
	{
	}
}
