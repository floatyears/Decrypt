using System;
using UnityEngine;

public class GuildCraftYuJiRankTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildCraftYuJiRankItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildCraftYuJiRankItem guildCraftYuJiRankItem = gameObject.AddComponent<GuildCraftYuJiRankItem>();
		guildCraftYuJiRankItem.InitWithBaseScene();
		return guildCraftYuJiRankItem;
	}
}
