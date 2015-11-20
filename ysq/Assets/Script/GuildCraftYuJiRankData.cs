using Proto;
using System;

public class GuildCraftYuJiRankData : BaseData
{
	public GuildWarRankData mRankData
	{
		get;
		private set;
	}

	public int mRankNum
	{
		get;
		private set;
	}

	public GuildCraftYuJiRankData(GuildWarRankData aInfo, int rNum)
	{
		this.mRankData = aInfo;
		this.mRankNum = rNum;
	}

	public override ulong GetID()
	{
		return (this.mRankData == null) ? 0uL : this.mRankData.GuildID;
	}
}
