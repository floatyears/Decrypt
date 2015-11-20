using Proto;
using System;

public class GuildJoinTabItemData : BaseData
{
	public BriefGuildData mBriefGuildData
	{
		get;
		private set;
	}

	public bool mIsRefreshBtn
	{
		get;
		private set;
	}

	public GuildJoinTabItemData(BriefGuildData bgd, bool isRefBtn)
	{
		this.mBriefGuildData = bgd;
		this.mIsRefreshBtn = isRefBtn;
	}

	public override ulong GetID()
	{
		return this.mBriefGuildData.ID;
	}
}
