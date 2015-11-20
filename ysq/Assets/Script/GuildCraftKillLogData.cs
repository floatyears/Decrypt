using Proto;
using System;

public class GuildCraftKillLogData : BaseData
{
	public GuildWarClientTeamMember mMemberData
	{
		get;
		private set;
	}

	public int mRankNum
	{
		get;
		private set;
	}

	public bool IsMVP
	{
		get;
		private set;
	}

	public GuildCraftKillLogData(GuildWarClientTeamMember tData, int rNum, bool isMvp)
	{
		this.mMemberData = tData;
		this.mRankNum = rNum;
		this.IsMVP = isMvp;
	}

	public override ulong GetID()
	{
		return (this.mMemberData == null) ? 0uL : this.mMemberData.Member.PlayerID;
	}
}
