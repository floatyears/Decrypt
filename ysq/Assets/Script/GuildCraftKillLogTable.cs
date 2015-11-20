using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildCraftKillLogTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildCraftKillLogItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildCraftKillLogItem guildCraftKillLogItem = gameObject.AddComponent<GuildCraftKillLogItem>();
		guildCraftKillLogItem.InitWithBaseScene();
		return guildCraftKillLogItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		GuildCraftKillLogData guildCraftKillLogData = (GuildCraftKillLogData)a;
		GuildCraftKillLogData guildCraftKillLogData2 = (GuildCraftKillLogData)b;
		if (guildCraftKillLogData != null && guildCraftKillLogData2 != null)
		{
			return guildCraftKillLogData.mRankNum - guildCraftKillLogData2.mRankNum;
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRankLvl));
	}
}
