using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildMemberItemsTable : UICustomGrid
{
	private int mSortType;

	public void SetSortType(int tp)
	{
		this.mSortType = tp;
		base.repositionNow = true;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildMemberItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildMemberItem guildMemberItem = gameObject.AddComponent<GuildMemberItem>();
		guildMemberItem.InitWithBaseScene();
		return guildMemberItem;
	}

	private int SortByMemberJob(BaseData a, BaseData b)
	{
		GuildMemberItemData guildMemberItemData = (GuildMemberItemData)a;
		GuildMemberItemData guildMemberItemData2 = (GuildMemberItemData)b;
		if (this.mSortType == 1)
		{
			return guildMemberItemData2.MemberData.TotalReputation - guildMemberItemData.MemberData.TotalReputation;
		}
		if (this.mSortType == 2)
		{
			return guildMemberItemData2.MemberData.Level - guildMemberItemData.MemberData.Level;
		}
		if (this.mSortType == 3)
		{
			return guildMemberItemData2.MemberData.LastOnlineTime - guildMemberItemData.MemberData.LastOnlineTime;
		}
		if (guildMemberItemData.MemberData.Rank < guildMemberItemData2.MemberData.Rank)
		{
			return -1;
		}
		if (guildMemberItemData.MemberData.Rank > guildMemberItemData2.MemberData.Rank)
		{
			return 1;
		}
		if (guildMemberItemData.MemberData.TotalReputation > guildMemberItemData2.MemberData.TotalReputation)
		{
			return -1;
		}
		if (guildMemberItemData.MemberData.TotalReputation < guildMemberItemData2.MemberData.TotalReputation)
		{
			return 1;
		}
		return guildMemberItemData2.MemberData.Level - guildMemberItemData.MemberData.Level;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByMemberJob));
	}
}
