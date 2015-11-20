using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildJoinTabItemTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildEnterItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildJoinTabItem guildJoinTabItem = gameObject.AddComponent<GuildJoinTabItem>();
		guildJoinTabItem.InitWithBaseScene();
		return guildJoinTabItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		GuildJoinTabItemData guildJoinTabItemData = (GuildJoinTabItemData)a;
		GuildJoinTabItemData guildJoinTabItemData2 = (GuildJoinTabItemData)b;
		if (guildJoinTabItemData == null || guildJoinTabItemData2 == null)
		{
			return 0;
		}
		if (guildJoinTabItemData.mIsRefreshBtn && !guildJoinTabItemData2.mIsRefreshBtn)
		{
			return 1;
		}
		if (!guildJoinTabItemData.mIsRefreshBtn && guildJoinTabItemData2.mIsRefreshBtn)
		{
			return -1;
		}
		if (guildJoinTabItemData.mBriefGuildData == null || guildJoinTabItemData2.mBriefGuildData == null)
		{
			return 0;
		}
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(guildJoinTabItemData.mBriefGuildData.Level);
		GuildInfo info2 = Globals.Instance.AttDB.GuildDict.GetInfo(guildJoinTabItemData2.mBriefGuildData.Level);
		if (info != null && info2 != null)
		{
			bool flag = guildJoinTabItemData.mBriefGuildData.MemberNum >= info.MaxMembers;
			bool flag2 = guildJoinTabItemData2.mBriefGuildData.MemberNum >= info2.MaxMembers;
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag && flag2)
			{
				return -1;
			}
		}
		if (guildJoinTabItemData.mBriefGuildData.Level > guildJoinTabItemData2.mBriefGuildData.Level)
		{
			return -1;
		}
		if (guildJoinTabItemData.mBriefGuildData.Level < guildJoinTabItemData2.mBriefGuildData.Level)
		{
			return 1;
		}
		if (guildJoinTabItemData.mBriefGuildData.MemberNum > guildJoinTabItemData2.mBriefGuildData.MemberNum)
		{
			return -1;
		}
		if (guildJoinTabItemData.mBriefGuildData.MemberNum < guildJoinTabItemData2.mBriefGuildData.MemberNum)
		{
			return 1;
		}
		return (int)guildJoinTabItemData.mBriefGuildData.ID - (int)guildJoinTabItemData2.mBriefGuildData.ID;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRankLvl));
	}
}
